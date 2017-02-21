using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Profile.BL.Interfaces;
using Profile.DAL.Entities;
using Profile.DAL.Identity;

namespace Profile.BL.Providers
{
    public class RoleProvider : IRoleProvider
    {
        private readonly UserManager _userManager;

        public RoleProvider(UserManager userManager)
        {
            _userManager = userManager;
        }

        public Task<IList<RoleType>> GetUserRoles(int userId)
        {
            return _userManager.GetRoleTypesAsync(userId);
        }
    }
}
