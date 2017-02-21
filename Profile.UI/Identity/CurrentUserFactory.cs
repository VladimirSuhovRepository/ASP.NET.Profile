using System.Security.Principal;

namespace Profile.UI.Identity
{
    public class CurrentUserFactory : ICurrentUserFactory
    {
        public AppUser CreateCurrentUser(IPrincipal principal)
        {
            return new AppUser(principal);
        }
    }
}