using System.Collections.Generic;
using Profile.BL.Interfaces;
using Profile.DAL.Entities;
using Profile.UI.Mappers;
using Profile.UI.Models.User;

namespace Profile.UI.Infrastructure.DetailedInfo
{
    public class ViewDetailsFactory
    {
        private const string HRDetailsViewName = "_HRDetailsView";

        private readonly IMentorProvider _mentorProvider;
        private readonly IScrumMasterProvider _scrumMasterProvider;

        private readonly MentorMapper _mentorMapper;
        private readonly ScrumMasterMapper _scrumMasterMapper;
        private readonly HRMapper _hrMapper;

        public ViewDetailsFactory(IMentorProvider mentorProvider,
                                       IScrumMasterProvider scrumMasterProvider,
                                       MentorMapper mentorMapper,
                                       ScrumMasterMapper scrumMasterMapper,
                                       HRMapper hrMapper)
        {
            _mentorProvider = mentorProvider;
            _scrumMasterProvider = scrumMasterProvider;

            _mentorMapper = mentorMapper;
            _scrumMasterMapper = scrumMasterMapper;
            _hrMapper = hrMapper;
        }

        public IList<IDetailedInfo> GetUserDetails(User user, IList<RoleType> userRoles)
        {
            var userDetails = new List<IDetailedInfo>();

            foreach (var role in userRoles)
            {
                switch (role)
                {
                    case RoleType.HR:
                        userDetails.Add(GetHRDetails(user));
                        break;

                    case RoleType.Manager:
                        userDetails.Add(new DetailedManagerInfo());
                        break;

                    case RoleType.Mentor:
                        userDetails.Add(GetMentorDetails(user.Id));
                        break;

                    case RoleType.ScrumMaster:
                        userDetails.Add(GetScrumMasterDetails(user.Id));
                        break;
                }
            }

            return userDetails;
        }

        private DetailedMentorInfo GetMentorDetails(int userId)
        {
            var mentor = _mentorProvider.Get(userId);

            return _mentorMapper.ToDetailedMentorInfo(mentor);
        }

        private DetailedScrumMasterInfo GetScrumMasterDetails(int userId)
        {
            var scrumMaster = _scrumMasterProvider.Get(userId);

            return _scrumMasterMapper.ToDetailedScrumMasterInfo(scrumMaster);
        }

        private DetailedHRInfo GetHRDetails(User user)
        {
            return _hrMapper.ToDetailedHRInfo(user.Contacts, HRDetailsViewName);
        }
    }
}