using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Profile.BL.Interfaces;
using Profile.DAL.Entities;
using Profile.UI.Identity;
using Profile.UI.Infrastructure.DetailedInfo;
using Profile.UI.Mappers;
using Profile.UI.Utility;

namespace Profile.UI.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUsersProvider _userProvider;
        private readonly IRoleProvider _roleProvider;
        private readonly IDetailedInfoBuilder _detailsBuilder;
        private readonly UserMapper _userMapper;

        public UserController(ICurrentUserFactory userFactory,
                              IUsersProvider userProvider,
                              IRoleProvider roleProvider,
                              IDetailedInfoBuilder detailsBuilder,
                              UserMapper userMapper)
            : base(userFactory)
        {
            _userProvider = userProvider;
            _roleProvider = roleProvider;
            _detailsBuilder = detailsBuilder;
            _userMapper = userMapper;
        }

        [HttpGet]
        public async Task<ActionResult> View(int? id)
        {
            int userId = id.GetValueOrDefault(CurrentUser.Id.Value);
            var user = await _userProvider.GetUserById(userId);
            var userRoles = await GetSortedUserRoles(userId);

            var userDetails = _detailsBuilder.GetUserDetails(user, 
                                                             userRoles, 
                                                             DetailedInfoType.View);

            bool isOwner = userId == CurrentUser.Id.Value;
            var userProfile = _userMapper.ToUserProfile(user, userDetails, isOwner);

            return View(userProfile);
        }

        private async Task<IList<RoleType>> GetUserRoles(int userId)
        {
            return userId == CurrentUser.Id ?
                CurrentUser.GetUserRoles() :
                await _roleProvider.GetUserRoles(userId);
        }

        private async Task<IList<RoleType>> GetSortedUserRoles(int userId)
        {
            var userRoles = await GetUserRoles(userId);

            var roleComparer = new RoleComparer(new Dictionary<RoleType, int>
            {
                { RoleType.Mentor, 1 },
                { RoleType.ScrumMaster, 2 }
            });

            return userRoles.OrderBy(r => r, roleComparer).ToList();
        }
    }
}