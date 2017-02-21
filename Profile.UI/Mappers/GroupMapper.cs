using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.Models;
using Profile.UI.Models.Manager;
using Profile.UI.Utility;

namespace Profile.UI.Mappers
{
    public class GroupMapper
    {
        private IMapper _mapper;

        public GroupMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ManagerTeamJsonResponseModel ToTeamManagerJsonModel(Group group)
        {
            var sortedTrainees = group.Trainees.OrderBy(t => t.Specialization, new SpecializationComparer()).ToList();
            return new ManagerTeamJsonResponseModel
            {
                ScrumName = group.ScrumMaster?.User.FullName,
                ProjectName = group.Project.Name,
                TeamName = group.Number,
                Trainees = sortedTrainees.Select(_mapper.Map<Trainee, ManagerTraineeJsonModel>).ToList()
            };
        }

        public GroupViewModel ToGroupViewModel(Group group)
        {
            var groupViewModel = _mapper.Map<Group, GroupViewModel>(group);

            return groupViewModel;
        }

        public Group FromGroupViewModel(GroupViewModel groupViewModel)
        {
            var group = _mapper.Map<GroupViewModel, Group>(groupViewModel);
            return group;
        }

        public ManagerTraineeMentorsJsonModel ToTraineeMentorsJsonModel(List<Trainee> trainees, 
                                                                                       List<Mentor> mentors)
        {
            return new ManagerTraineeMentorsJsonModel
            {
                Trainees = trainees.Select(t => _mapper.Map<User, UserNameCommonModel>(t.User)).OrderBy(t => t.FullName).ToList(),
                Mentors = mentors.Select(m => _mapper.Map<User, UserNameCommonModel>(m.User)).OrderBy(m => m.FullName).ToList()
            };
        }

        public TeamProjectInputViewModel ToFormDataAddProjectViewModel(List<Specialization> specializations,
                                                                           List<ScrumMaster> scrumMasters)
        {
            var sortedSpecializations = specializations.OrderBy(s => s, new SpecializationComparer()).ToList();
            var teamInfoViewModel = new TeamProjectInputViewModel
            {
                ScrumMasters = scrumMasters.Select(sm => _mapper.Map<ScrumMaster, ScrumHasProjectViewModel>(sm))
                                           .OrderBy(sm => sm.FullName).ToList(),

                Specializations = sortedSpecializations.Select(_mapper.Map<Specialization, SpecializationNameViewModel>)
                                                 .ToList()
            };

            return teamInfoViewModel;
        }

        public TeamProjectInputViewModel ToUpdateProjectViewModel(List<Trainee> trainees,
                                                                      List<Mentor> mentors,
                                                                      List<Specialization> specializations,
                                                                      List<ScrumMaster> scrumMasters,
                                                                      Group group)
        {
            var formDataViewModel = ToFormDataAddProjectViewModel(specializations, scrumMasters);
            formDataViewModel.Project = _mapper.Map<Project, ProjectViewModel>(group.Project);
            formDataViewModel.Group = _mapper.Map<Group, GroupWithTraineesViewModel>(group);
            formDataViewModel.Group.Trainees = group.Trainees.OrderBy(t => t.Specialization, new SpecializationComparer())
                                                             .Select(_mapper.Map<Trainee, TraineeViewModel>).ToList();
            formDataViewModel.TraineesForSelect = trainees.Select(t => _mapper.Map<Trainee, TraineeViewModel>(t))
                                                          .OrderBy(t => t.FullName)
                                                          .ToList();
            formDataViewModel.MentorsForSelect = mentors.Select(_mapper.Map<Mentor, UserBySpecialization>).OrderBy(m => m.FullName).ToList();
            return formDataViewModel;
        }

        public Group ManagerPostTeamJsonModelToGroup(TeamPostJsonModel jsonGroup)
        {
            var group = _mapper.Map<TeamPostJsonModel, Group>(jsonGroup);

            return group;
        }
    }
}