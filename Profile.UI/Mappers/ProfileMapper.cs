using System.Linq;
using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.Models.Json;
using Profile.UI.Models.Profile;
using Profile.UI.Models.Review;

namespace Profile.UI.Mappers
{
    public class ProfileMapper
    {
        private IMapper _mapper;

        public ProfileMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ContactsViewModel ToContactsViewModel(TraineeProfile profile)
        {
            var contactsViewModel = _mapper.Map<Contacts, ContactsViewModel>(profile.Trainee.User.Contacts);

            return contactsViewModel;
        }

        public PositionViewModel ToPositionViewModel(TraineeProfile profile)
        {
            var positionViewModel = _mapper.Map<TraineeProfile, PositionViewModel>(profile);

            return positionViewModel;
        }

        public JobViewModel ToJobViewModel(TraineeProfile profile)
        {
            var jobViewModel = _mapper.Map<TraineeProfile, JobViewModel>(profile);

            return jobViewModel;
        }

        public QualificationViewModel ToQualificationViewModel(TraineeProfile profile)
        {
            var qualificationViewModel = _mapper.Map<TraineeProfile, QualificationViewModel>(profile);

            return qualificationViewModel;
        }

        public FileViewModel ToFileViewModel(File file)
        {
            var fileViewModel = _mapper.Map<File, FileViewModel>(file);

            return fileViewModel;
        }

        public LinkViewModel ToLinkViewModel(Link link)
        {
            var linksViewModel = _mapper.Map<Link, LinkViewModel>(link);

            return linksViewModel;
        }

        public ArtefactsViewModel ToArtefactsViewModel(TraineeProfile profile)
        {
            var filesViewModel = profile.Files.Select(ToFileViewModel).ToList();
            var linksViewModel = profile.Links.Select(ToLinkViewModel).ToList();

            var artefactsViewModel = new ArtefactsViewModel
            {
                Files = filesViewModel,
                Links = linksViewModel
            };

            return artefactsViewModel;
        }

        public ProfileReviewViewModel ToProfileReviewViewModel(
            TraineeProfile profile,
            double traineeRating,
            bool isTraineeHavingMentorReview,
            bool isTraineeHavingScrumReview,
            bool isTraineeHavingTeamReview)
        {
            var profileReviewViewModel = new ProfileReviewViewModel
            {
                TraineeId = profile.Id,
                TraineeFullName = profile.Trainee.User.FullName,
                ProjectId = profile.Trainee.Group.Project.Id,
                ProjectName = profile.Trainee.Group.Project.Name,
                SpecializationId = profile.Trainee.Specialization.Id,
                SpecializationName = profile.Trainee.Specialization.Name,
                GpoupId = profile.Trainee.Group.Id,
                GroupNumber = profile.Trainee.Group.Number,
                Rating = traineeRating,
                HasReviews = profile.Trainee.ReviewsOnMe.Any(),
                IsTraineeHavingMentorReview = isTraineeHavingMentorReview,
                IsTraineeHavingScrumReview = isTraineeHavingScrumReview,
                IsTraineeHavingTeamReview = isTraineeHavingTeamReview
            };

            return profileReviewViewModel;
        }

        public ProfileViewModel ToProfileViewModel(TraineeProfile profile, double traineeRating)
        {
            var profileViewModel = new ProfileViewModel
            {
                Id = profile.Id,
                TraineeId = profile.Trainee.Id,
                TraineeFullName = profile.Trainee.User.FullName,
                ProjectId = profile.Trainee.Group.Project.Id,
                ProjectName = profile.Trainee.Group.Project.Name,
                SpecializationId = profile.Trainee.Specialization.Id,
                SpecializationName = profile.Trainee.Specialization.Name,
                GpoupId = profile.Trainee.Group.Id,
                GroupNumber = profile.Trainee.Group.Number,
                Rating = traineeRating,
                HasReviews = profile.Trainee.ReviewsOnMe.Any(),

                Contacts = ToContactsViewModel(profile),
                Position = ToPositionViewModel(profile),
                Job = ToJobViewModel(profile),
                Qualification = ToQualificationViewModel(profile),

                Artefacts = ToArtefactsViewModel(profile)
            };

            return profileViewModel;
        }

        public TraineeProfile FromProfileMainInfoJsonModel(ProfileMainInfoJson profileJson)
        {
            var profile = _mapper.Map<ProfileMainInfoJson, TraineeProfile>(profileJson);

            profile.Trainee = new Trainee
            {
                User = new User
                {
                    Contacts = new Contacts
                    {
                        Email = profileJson.Email,
                        Phone = profileJson.Phone,
                        LinkedIn = profileJson.LinkedIn,
                        Skype = profileJson.Skype
                    }
                }
            };

            return profile;
        }

        public ProfileMainInfoJson ToJsonProfileMainInfoModel(TraineeProfile profile)
        {
            var profileJson = _mapper.Map<TraineeProfile, ProfileMainInfoJson>(profile);

            _mapper.Map(profile.Trainee.User.Contacts, profileJson);

            return profileJson;
        }

        public TraineeProfile FromProfileQualificationJsonModel(ProfileQualificationJson profileJson)
        {
            var profile = _mapper.Map<ProfileQualificationJson, TraineeProfile>(profileJson);

            return profile;
        }

        public ProfileQualificationJson ToProfileQualificationJsonModel(TraineeProfile updatedProfile)
        {
            var profileJson = _mapper.Map<TraineeProfile, ProfileQualificationJson>(updatedProfile);

            return profileJson;
        }
    }
}