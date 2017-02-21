using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Profile.UI.Tests.Owin.OwinTestStartup))]

namespace Profile.UI.Tests.Owin
{
    public class OwinTestStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.Use<LoginMiddleware>();
        }
    }
}
