using Microsoft.AspNet.Identity.EntityFramework;
using Profile.DAL.Context;
using Profile.DAL.Entities;
using Profile.DAL.Identity.Entities;

namespace Profile.DAL.Identity
{
    public class RoleStore : RoleStore<Role, int, UserRole>
    {
        public RoleStore(ProfileContext context) 
            : base(context)
        {
        }
    }
}
