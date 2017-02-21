using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Profile.DAL.Entities;

namespace Profile.UI.Identity
{
    public class AppUser : ClaimsPrincipal
    {
        private const string PermissionClaimType = "Permission";

        private IList<RoleType> _roles;
        private IList<PermissionType> _permissions;

        public AppUser(IPrincipal principal)
            : base(principal)
        {
            _roles = GetRoles();
            _permissions = GetPermissions();
        }

        protected AppUser()
        {
            _roles = new List<RoleType>();
            _permissions = new List<PermissionType>();
        }

        public int? Id => GetUserId();
        public virtual bool IsAuthenticated => Identity.IsAuthenticated;

        protected IList<RoleType> Roles => _roles;

        public IList<RoleType> GetUserRoles()
        {
            return _roles.ToList();
        }

        public bool HasRole(RoleType role)
        {
            return _roles.Contains(role);
        }

        public bool HasPermission(PermissionType permission)
        {
            return _permissions.Contains(permission);
        }

        protected virtual int? GetUserId()
        {
            int id;
            string idClaim = FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            if (int.TryParse(idClaim, out id)) return id;

            return null;
        }

        private IList<RoleType> GetRoles()
        {
            return GetParsedClaims<RoleType>(ClaimTypes.Role);
        }

        private IList<PermissionType> GetPermissions()
        {
            return GetParsedClaims<PermissionType>(PermissionClaimType);
        }

        private IList<TItem> GetParsedClaims<TItem>(string claimType)
            where TItem : struct
        {
            var items = new List<TItem>();
            var claims = FindAll(claimType);

            foreach (var claim in claims)
            {
                TItem item;
                bool isParsed = Enum.TryParse(claim.Value, out item);

                if (isParsed) items.Add(item);
            }

            return items;
        }
    }
}