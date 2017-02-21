using System.Linq;
using AutoMapper;
using Profile.BL.Models;
using Profile.DAL.Entities;
using Profile.UI.Models;
using Profile.UI.Models.Manager;

namespace Profile.UI.Mappers
{
    public class TraineeMapper
    {
        private IMapper _mapper;

        public TraineeMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TraineeViewModel ToTraineeViewModel(Trainee trainee, int reviewerId = 0)
        {
            var mentorProfileViewModel = new TraineeViewModel
            {
                Id = trainee.Id,
                FullName = trainee.User.FullName,
                Specialization = trainee.Specialization.Name,
                Project = trainee.Group.Project.Name,
                ReleaseDuration = GetReleaseDuration(trainee.Group),
                Command = trainee.Group.Number,
                IsAllowed = trainee.IsAllowed,
                IsReviewed = trainee.ReviewsOnMe.Any(t => t.ReviewerId == reviewerId)
            };

            return mentorProfileViewModel;
        }

        public TraineeToMentorId TraineeJsonToTraineeMentorId(ManagerTraineeJsonModel traineeJson)
        {
            var trainee = _mapper.Map<ManagerTraineeJsonModel, TraineeToMentorId>(traineeJson);

            return trainee;
        }

        private string GetReleaseDuration(Group group)
        {
            return string.Format("{0:dd.MM.yyyy} - {1:dd.MM.yyyy}", group.StartDate, group.FinishDate);
        }
    }
}