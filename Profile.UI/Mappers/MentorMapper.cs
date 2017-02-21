using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.Models.Json;
using Profile.UI.Models.Mentor;
using Profile.UI.Models.User;

namespace Profile.UI.Mappers
{
    public class MentorMapper
    {
        private IMapper _mapper;

        public MentorMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Mentor FromMentorEditRequestJsonModel(MentorEditRequestJsonModel mentorEditRequestJsonModel)
        {
            var mentor = new Mentor
            {
                Id = mentorEditRequestJsonModel.Id,

                User = new User
                {
                    FullName = mentorEditRequestJsonModel.FullName,

                    Contacts = new Contacts
                    {
                        UserId = mentorEditRequestJsonModel.Id,
                        Email = mentorEditRequestJsonModel.Email,
                        Phone = mentorEditRequestJsonModel.Phone,
                        Skype = mentorEditRequestJsonModel.Skype,
                        LinkedIn = mentorEditRequestJsonModel.LinkedIn
                    }
                }
            };

            return mentor;
        }

        public MentorProfileViewModel ToMentorProfileViewModel(Mentor mentor)
        {
            var mentorProfileViewModel = new MentorProfileViewModel
            {
                Id = mentor.Id,
                FullName = mentor.User.FullName,
                Specialization = mentor.Specialization.Name,
                Email = mentor.User.Contacts.Email,
                Phone = mentor.User.Contacts.Phone,
                Skype = mentor.User.Contacts.Skype,
                LinkedIn = mentor.User.Contacts.LinkedIn,
                AvatarUrl = mentor.User.Avatar.Base64Url
            };

            return mentorProfileViewModel;
        }

        public MentorEditResponseJsonModel ToMentorEditResponseJsonModel(Mentor mentor)
        {
            var mentorEditResponseJsonModel = new MentorEditResponseJsonModel
            {
                Id = mentor.Id,
                FullName = mentor.User.FullName,
                Email = mentor.User.Contacts.Email,
                Phone = mentor.User.Contacts.Phone,
                Skype = mentor.User.Contacts.Skype,
                LinkedIn = mentor.User.Contacts.LinkedIn,
                AvatarUrl = mentor.User.Avatar.Base64Url
            };

            return mentorEditResponseJsonModel;
        }

        public DetailedMentorInfo ToDetailedMentorInfo(Mentor mentor)
        {
            return _mapper.Map<DetailedMentorInfo>(mentor);
        }
    }
}