using System.Collections.Generic;
using Profile.DAL.Entities;
using Profile.UI.Models.User;

namespace Profile.UI.Infrastructure.DetailedInfo
{
    public class DetailedInfoBuilder : IDetailedInfoBuilder
    {
        private readonly ViewDetailsFactory _viewFactory;
        private readonly EditDetailsFactory _editFactory;

        public DetailedInfoBuilder(ViewDetailsFactory viewFactory,
                                   EditDetailsFactory editFactory)
        {
            _viewFactory = viewFactory;
            _editFactory = editFactory;
        }

        public IList<IDetailedInfo> GetUserDetails(User user, 
                                                   IList<RoleType> userRoles, 
                                                   DetailedInfoType detailsType)
        {
            if (detailsType == DetailedInfoType.Edit)
            {
                return _editFactory.GetUserDetails(user, userRoles);
            }

            return _viewFactory.GetUserDetails(user, userRoles);
        }
    }
}