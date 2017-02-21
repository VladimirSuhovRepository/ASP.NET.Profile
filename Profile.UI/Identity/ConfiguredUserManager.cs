using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ninject.Activation;
using Profile.DAL.Entities;
using Profile.DAL.Identity;

namespace Profile.UI.Identity
{
    public class ConfiguredUserManager : UserManager
    {
        public ConfiguredUserManager(IUserStore<User, int> userStore,
                                     IPermissionStore<Permission, int> permissionStore,
                                     IIdentityMessageService emailService,
                                     IdentityFactoryOptions<UserManager> options)
            : base(userStore, permissionStore, emailService)
        {
            var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider != null)
            {
                UserTokenProvider = new DataProtectorTokenProvider<User, int>(
                    dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }
}