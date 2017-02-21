using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.DAL.Identity
{
    public class PermissionStore : IPermissionStore<Permission, int>
    {
        private readonly IProfileContext _context;

        public PermissionStore(IProfileContext context)
        {
            _context = context;
        }

        public async Task<IList<Permission>> GetUserPermissions(int userId)
        {
            return await _context.Roles
                .Where(r => r.Users.Any(ur => ur.UserId == userId))
                .SelectMany(r => r.Permissions)
                .ToListAsync();
        }
    }
}
