using System.Linq;
using System.Web.Mvc;
using Profile.BL.Interfaces;
using Profile.DAL.Entities;
using Profile.UI.Identity;
using Profile.UI.Mappers;
using Profile.UI.Utility;

namespace Profile.UI.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectProvider _projectProvider;
        private readonly ProjectMapper _projectMapper;

        public ProjectController(IProjectProvider projectProvider, 
                                 ProjectMapper projectMapper,
                                 ICurrentUserFactory userFactory)
            : base(userFactory)
        {
            _projectProvider = projectProvider;
            _projectMapper = projectMapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult List()
        {
            var projects = _projectProvider.GetAll();

            var projectsViewModel = projects.Select(_projectMapper.ToViewModel)
                .OrderByDescending(p => p.StartDate)
                .ToList();

            return View(projectsViewModel);
        }
        
        public ActionResult View(int id)
        {
            var project = _projectProvider.GetProject(id);

            if (project.Status != ProjectStatus.InProgress)
            {
                return new HttpNotFoundResult();
            }

            project.Groups = project.Groups.Where(g => g.HasScrumMaster)
                .OrderBy(g => g.StartDate)
                .ToList();

            foreach (var group in project.Groups)
            {
                group.Trainees = group.Trainees
                    .OrderBy(t => t.Specialization, new SpecializationComparer())
                    .ToList();
            }

            var isCurrentUserOwner = CurrentUser.HasRole(RoleType.ScrumMaster) ?
                project.IsScrumMasterOwner(CurrentUser.Id.Value) :
                false;

            var viewModel = _projectMapper.ToProjectDescriptionViewModel(project, isCurrentUserOwner);

            return View(viewModel);
        }
    }
}