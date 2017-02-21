using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Profile.UI.Tests.Owin
{
    internal class LoginMiddleware : OwinMiddleware
    {
        public LoginMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            context.Response.StatusCode = Convert.ToInt16(HttpStatusCode.OK);

            return Task.FromResult(0);
        }
    }
}
