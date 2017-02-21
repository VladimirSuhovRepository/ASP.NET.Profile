using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Profile.DAL.Identity
{
    public interface IPermissionStore<TPermission, TKey>
        where TPermission : class
        where TKey : IEquatable<TKey>
    {
        Task<IList<TPermission>> GetUserPermissions(TKey userId);
    }
}
