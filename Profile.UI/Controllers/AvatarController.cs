using System.Web.Mvc;
using Profile.BL.Interfaces;
using Profile.UI.Identity;

namespace Profile.UI.Controllers
{
    [AllowAnonymous]
    public class AvatarController : BaseController
    {
        private readonly IUsersProvider _userProvider;

        public AvatarController(
            IUsersProvider userProvider,
            ICurrentUserFactory userFactory)
            : base(userFactory)
        {
            _userProvider = userProvider;
        }

        public string GetUserAvatar()
        {
            if (CurrentUser.Id.HasValue)
            {
                return _userProvider.GetAvatarByUserIdOrDefault(CurrentUser.Id.Value).Base64Url;
            }

            return _userProvider.GetDefaultAvatarUrl();
        }
    }
}