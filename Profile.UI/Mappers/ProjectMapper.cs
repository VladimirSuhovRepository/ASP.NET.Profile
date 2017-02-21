using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.Models;
using Profile.UI.Models.Manager;
using Profile.UI.Models.Project;

namespace Profile.UI.Mappers
{
    public class ProjectMapper
    {
        private IMapper _mapper;

        public ProjectMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ProjectViewModel ToViewModel(Project project)
        {
            return _mapper.Map<Project, ProjectViewModel>(project);
        }

        public ProjectTraineeViewModel ToProjectTraineeViewModel(Trainee trainee)
        {
            var projectViewModel = new ProjectTraineeViewModel
            {
                Id = trainee.Id,
                FullName = trainee.User.FullName,
                IsAllowed = trainee.IsAllowed,
                IsReviewed = trainee.ReviewsOnMe
                    .Any(r => r.ReviewerId == trainee.Mentor.Id),

                Project = new ProjectViewModel
                {
                    Id = trainee.Group.Project.Id,
                    Name = trainee.Group.Project.Name,
                    ShortDescription = trainee.Group.Project.ShortDescription
                }
            };

            return projectViewModel;
        }

        public ManagerProjectViewModel ToManagerProjetsViewModel(Project project)
        {
            var viewModel = _mapper.Map<Project, ManagerProjectViewModel>(project);
            viewModel.Groups = project.Groups.Select(_mapper.Map<Group, GroupViewModel>).OrderBy(g => g.Number).ToList();

            return viewModel;
        }

        public Project TeamProjectPostJsonModelToProject(TeamProjectPostJsonModel projectJson)
        {
            var project = _mapper.Map<TeamProjectPostJsonModel, Project>(projectJson);

            return project;
        }

        public ManagerProjectsMenuViewModel ToManagerProjectsMenuViewModel(List<ManagerProjectViewModel> projects, int newTeamId = 0)
        {
            var project = new ManagerProjectsMenuViewModel(newTeamId, projects);

            return project;
        }

        public ProjectDescriptionViewModel ToProjectDescriptionViewModel(Project project,
                                                                         bool isCurrentUserOwner)
        {
            var projectViewModel = _mapper.Map<Project, ProjectDescriptionViewModel>(project);

            projectViewModel.IsCurrentUserOwner = isCurrentUserOwner;

            return projectViewModel;
        }

        public Project FromProjectViewModel(ProjectViewModel projectViewModel)
        {
            var project = _mapper.Map<ProjectViewModel, Project>(projectViewModel);

            return project;
        }
    }
}