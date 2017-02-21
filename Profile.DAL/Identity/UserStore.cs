using Microsoft.AspNet.Identity.EntityFramework;
using Profile.DAL.Context;
using Profile.DAL.Entities;
using Profile.DAL.Identity.Entities;

namespace Profile.DAL.Identity
{
    public class UserStore : UserStore<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public UserStore(ProfileContext context)
            : base(context)
        {
        }
    }
}
