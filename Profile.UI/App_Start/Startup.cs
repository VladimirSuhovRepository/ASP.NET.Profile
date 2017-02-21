using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Profile.DAL.Entities;
using Profile.DAL.Identity;

[assembly: OwinStartup(typeof(Profile.UI.App_Start.Startup))]

namespace Profile.UI.App_Start
{
    public class Startup
    { 
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),

                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator
                    .OnValidateIdentity<UserManager, User, int>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentityCallback: (manager, user) =>
                            user.GenerateUserIdentityAsync(manager),
                        getUserIdCallback: id => id.GetUserId<int>())
                }
            });
        }
    }
}