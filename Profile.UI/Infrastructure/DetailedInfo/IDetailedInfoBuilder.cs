using System.Collections.Generic;
using Profile.DAL.Entities;
using Profile.UI.Models.User;

namespace Profile.UI.Infrastructure.DetailedInfo
{
    public interface IDetailedInfoBuilder
    {
        IList<IDetailedInfo> GetUserDetails(User user, 
                                            IList<RoleType> userRoles,
                                            DetailedInfoType detailsType);
    }
}