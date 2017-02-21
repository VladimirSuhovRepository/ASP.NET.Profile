using System.Linq;
using Profile.BL.Providers;
using Profile.DAL.Context;
using Profile.DAL.Entities;
using Profile.DAL.Identity;
using Profile.DAL.Identity.Entities;

namespace Profile.UI.Tests.Providers
{
    public class FakeUsersProvider : UsersProvider
    {
        public FakeUsersProvider(IProfileContext profileContext, UserManager userManager) : base(profileContext, userManager)
        {
        }

        protected override void AddToRole(User user, RoleType roleType)
        {
            var role = _context.Roles.Single(r => r.Type == roleType);
            role.Users.Add(new UserRole { RoleId = role.Id, UserId = user.Id });
        }
    }
}
