using Microsoft.AspNet.Identity;
using Profile.DAL.Entities;

namespace Profile.DAL.Identity
{
    public class RoleManager : RoleManager<Role, int>
    {
        public RoleManager(IRoleStore<Role, int> store) 
            : base(store)
        {
        }
    }
}
