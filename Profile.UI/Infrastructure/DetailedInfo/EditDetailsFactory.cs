using System.Collections.Generic;
using Profile.DAL.Entities;
using Profile.UI.Mappers;
using Profile.UI.Models.User;

namespace Profile.UI.Infrastructure.DetailedInfo
{
    public class EditDetailsFactory
    {
        private const string HRDetailsViewName = "_HRDetailsEdit";

        private readonly HRMapper _hrMapper;

        public EditDetailsFactory(HRMapper hrMapper)
        {
            _hrMapper = hrMapper;
        }

        public IList<IDetailedInfo> GetUserDetails(User user, IList<RoleType> userRoles)
        {
            var userDetails = new List<IDetailedInfo>();

            if (userRoles.Contains(RoleType.HR))
            {
                var hrDetails = _hrMapper.ToDetailedHRInfo(user.Contacts, HRDetailsViewName);

                userDetails.Add(hrDetails);
            }

            return userDetails;
        }
    }
}