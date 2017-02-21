using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Profile.DAL.Entities;

namespace Profile.DAL.Identity
{
    public class UserManager : UserManager<User, int>
    {
        private readonly IPermissionStore<Permission, int> _permissionStore;

        public UserManager(
            IUserStore<User, int> userStore, 
            IPermissionStore<Permission, int> permissionStore,
            IIdentityMessageService emailService)
            : base(userStore)
        {
            _permissionStore = permissionStore;
            EmailService = emailService;
        }

        public override async Task<string> GeneratePasswordResetTokenAsync(int userId)
        {
            string token = await base.GeneratePasswordResetTokenAsync(userId);
            string encodedToken = Uri.EscapeDataString(token);

            return encodedToken;
        }

        public override Task<IdentityResult> ResetPasswordAsync(int userId, string token, string newPassword)
        {
            string decodedToken = Uri.UnescapeDataString(token);

            return base.ResetPasswordAsync(userId, decodedToken, newPassword);
        }

        public override async Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType)
        {
            var claims = await base.CreateIdentityAsync(user, authenticationType);
            var permissions = await _permissionStore.GetUserPermissions(user.Id);

            foreach (var permission in permissions)
            {
                claims.AddClaim(new Claim("Permission", permission.Type.ToString()));
            }

            return claims;
        }

        public Task<IdentityResult> AddToRoleAsync(int userId, RoleType roleType)
        {
            return AddToRoleAsync(userId, roleType.ToString());
        }

        public async Task<IList<RoleType>> GetRoleTypesAsync(int userId)
        {
            var roles = await GetRolesAsync(userId);
            var enumType = typeof(RoleType);

            return roles.Select(r => (RoleType)Enum.Parse(enumType, r)).ToList();
        }
    }
}
