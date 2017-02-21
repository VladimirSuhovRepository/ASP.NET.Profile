using System;
using System.Linq;
using System.Web.Mvc;
using Profile.BL.Interfaces;
using Profile.DAL.Entities;
using Profile.UI.Filters;
using Profile.UI.Mappers;
using Profile.UI.ModelEnums;
using Profile.UI.Models.Manager;

namespace Profile.UI.Controllers
{
    [PermissionAuthorize(Permission = PermissionType.OnlyManager)]
    public class ManagerController : Controller
    {
        private readonly IGroupProvider _groupProvider;
        private readonly IProjectProvider _projectProvider;
        private readonly IScrumMasterProvider _scrumMasterProvider;
        private readonly ITraineeProvider _traineeProvider;
        private readonly TraineeMapper _traineeMapper;
        private readonly IMentorProvider _mentorProvider;
        private readonly ISpecializationProvider _specializationProvider;
        private readonly IUsersProvider _userProvider;
        private readonly ProjectMapper _projectMapper;
        private readonly GroupMapper _groupMapper;
        private readonly MentorMapper _mentorMapper;
        private readonly UserMapper _userMapper;
        private readonly RoleMapper _roleMapper;

        public ManagerController(IGroupProvider groupProvider,
                                 IProjectProvider projectProvider,
                                 IScrumMasterProvider scrumMasterProvider,
                                 ITraineeProvider traineeProvider,
                                 TraineeMapper traineeMapper,
                                 IMentorProvider mentorProvider,
                                 ISpecializationProvider specializationProvider,
                                 IUsersProvider userProvider,
                                 ProjectMapper projectMapper,
                                 GroupMapper groupMapper,
                                 MentorMapper mentorMapper,
                                 UserMapper userMapper,
                                 RoleMapper roleMapper)
        {
            _groupProvider = groupProvider;
            _projectProvider = projectProvider;
            _scrumMasterProvider = scrumMasterProvider;
            _traineeProvider = traineeProvider;
            _traineeMapper = traineeMapper;
            _mentorProvider = mentorProvider;
            _specializationProvider = specializationProvider;
            _userProvider = userProvider;
            _projectMapper = projectMapper;
            _groupMapper = groupMapper;
            _mentorMapper = mentorMapper;
            _userMapper = userMapper;
            _roleMapper = roleMapper;
        }

        public ActionResult ManageRoles()
        {
            var newUsers = _userProvider.GetNewUsers();
            var specializations = _specializationProvider.GetAll();
            var newUsersViewModel = _userMapper.ToNewUsersSetRolesViewModel(newUsers, specializations);

            return View(newUsersViewModel);
        }

        [HttpPost]
        public ActionResult SetRoles(NewUsersToRolesJsonModel usersJson)
        {
            var roles = _roleMapper.ToDomainRoleTypes(usersJson.Role);
            var usersIds = usersJson.UserIds;
            var specializationId = usersJson.SpecializationId;
            _userProvider.SetRoles(usersIds, roles, specializationId);

            return Content("Success");
        }

        [HttpPost]
        public ActionResult DeleteUsers(NewUsersForRemovingJsonModel usersJson)
        {
            var usersIds = usersJson.UserIds;
            _userProvider.DeleteNewUsers(usersIds);

            return new EmptyResult();
        }

        public ActionResult ManageProjects()
        {
            var projectsMenuViewModel = GetProjectsMenuModel();

            return View(projectsMenuViewModel);
        }

        public ActionResult GetProjectsMenu(int teamId)
        {            
            var projectsMenuViewModel = GetProjectsMenuModel(teamId);

            return PartialView("_PartialProjectsMenu", projectsMenuViewModel);
        }

        public JsonResult GetTeam(int id)
        {
            var group = _groupProvider.GetGroup(id);
            var jsonModel = _groupMapper.ToTeamManagerJsonModel(group);
            return Json(jsonModel);
        }

        public JsonResult GetTraineesAndMentors(int specializationId)
        {
            var trainees = _traineeProvider.GetFreeTraineesBySpecialization(specializationId);
            var mentors = _mentorProvider.GetMentorsBySpecialization(specializationId);
            var jsonListMentorsAndTrainees = _groupMapper.ToTraineeMentorsJsonModel(trainees, mentors);

            return Json(jsonListMentorsAndTrainees);
        }

        public ActionResult AddNewProject()
        {
            var scrumMasters = _scrumMasterProvider.GetAll();
            var specializations = _specializationProvider.GetAll();
            var inputDataTeamViewModel = _groupMapper.ToFormDataAddProjectViewModel(specializations, scrumMasters);
            inputDataTeamViewModel.FormType = ManagerFormType.AddProjectAndTeam;

            return PartialView("_PartialProjectEditorForm", inputDataTeamViewModel);
        }

        [HttpPost]
        public JsonResult AddNewProject(TeamProjectPostJsonModel projectJson)
        {
            var project = _projectMapper.TeamProjectPostJsonModelToProject(projectJson);
            var group = _groupMapper.ManagerPostTeamJsonModelToGroup(projectJson.Team);
            var trainees = projectJson.Team.Trainees.Select(_traineeMapper.TraineeJsonToTraineeMentorId).ToList();
            var newProject = _projectProvider.AddProject(project);
            var newGroup = _groupProvider.AddGroup(trainees, group, newProject);

            return Json(newGroup.Id);
        }

        public ActionResult UpdateTeamProject(int teamId)
        {
            var group = _groupProvider.GetGroup(teamId);
            var scrumMasters = _scrumMasterProvider.GetAll();
            var specializations = _specializationProvider.GetAll();
            var trainees = _traineeProvider.GetFreeTrainees();
            trainees.AddRange(group.Trainees);
            var mentors = _mentorProvider.GetAll();
            var formDataViewModel = _groupMapper.ToUpdateProjectViewModel(trainees,
                                                                          mentors,
                                                                          specializations, 
                                                                          scrumMasters, 
                                                                          group);
            formDataViewModel.FormType = ManagerFormType.UpdateProjectAndTeam;

            return PartialView("_PartialProjectEditorForm", formDataViewModel);
        }

        [HttpPost]
        public JsonResult UpdateTeamProject(TeamProjectPostJsonModel projectJson)
        {
            var group = _groupMapper.ManagerPostTeamJsonModelToGroup(projectJson.Team);
            group.Project = _projectMapper.TeamProjectPostJsonModelToProject(projectJson);
            var trainees = projectJson.Team.Trainees.Select(_traineeMapper.TraineeJsonToTraineeMentorId).ToList();
            var updatedGroup = _groupProvider.UpdateGroup(trainees, group);

            return Json(updatedGroup.Id);
        }

        public ActionResult Delete(int teamId)
        {
            var group = _groupProvider.GetGroup(teamId);
            _projectProvider.SendToArchive(group.Project);

            var projectsMenuViewModel = GetProjectsMenuModel();

            return PartialView("_PartialProjectsMenu", projectsMenuViewModel);
        }

        public ActionResult AddTeam(int projectId)
        {
            var scrumMasters = _scrumMasterProvider.GetAll();
            var specializations = _specializationProvider.GetAll();
            var project = _projectProvider.GetProject(projectId);
            var inputDataTeamViewModel = _groupMapper.ToFormDataAddProjectViewModel(specializations, scrumMasters);
            inputDataTeamViewModel.Project = _projectMapper.ToViewModel(project);
            inputDataTeamViewModel.FormType = ManagerFormType.AddTeam;

            return PartialView("_PartialProjectEditorForm", inputDataTeamViewModel);
        }

        [HttpPost]
        public ActionResult AddTeam(TeamProjectPostJsonModel teamJson)
        {
            var project = _projectProvider.GetProject(teamJson.ProjectId);
            var group = _groupMapper.ManagerPostTeamJsonModelToGroup(teamJson.Team);
            var trainees = teamJson.Team.Trainees.Select(_traineeMapper.TraineeJsonToTraineeMentorId).ToList();
            var newGroup = _groupProvider.AddGroup(trainees, group, project);

            return Json(newGroup.Id);
        }

        private ManagerProjectsMenuViewModel GetProjectsMenuModel(int teamId = 0)
        {
            var projects = _projectProvider.GetActive().OrderBy(p => p.Name);
            var managerProjects = projects.Select(_projectMapper.ToManagerProjetsViewModel).ToList();
            return _projectMapper.ToManagerProjectsMenuViewModel(managerProjects, teamId);
        }
    }
}