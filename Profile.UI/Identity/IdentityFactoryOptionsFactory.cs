using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Ninject.Activation;
using Profile.DAL.Identity;

namespace Profile.UI.Identity
{
    public static class IdentityFactoryOptionsFactory
    {
        public static IdentityFactoryOptions<UserManager> Create(IContext context)
        {
            var options = new IdentityFactoryOptions<UserManager>
            {
                DataProtectionProvider = new DpapiDataProtectionProvider("Profile")
            };

            return options;
        }
    }
}