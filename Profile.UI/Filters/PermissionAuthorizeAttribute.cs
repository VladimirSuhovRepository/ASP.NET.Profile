using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Profile.DAL.Entities;
using Profile.UI.Identity;

namespace Profile.UI.Filters
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public PermissionType Permission { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = new AppUser(httpContext.User as ClaimsPrincipal);

            return base.AuthorizeCore(httpContext) && HasAllowedPermission(user);
        }

        private bool HasAllowedPermission(AppUser user)
        {
            return Permission == PermissionType.All ? true : user.HasPermission(Permission);
        }
    }
}