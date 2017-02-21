using System.Collections.Generic;
using System.Threading.Tasks;
using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface IRoleProvider
    {
        Task<IList<RoleType>> GetUserRoles(int userId);
    }
}
