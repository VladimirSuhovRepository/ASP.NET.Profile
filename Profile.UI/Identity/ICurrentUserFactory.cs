using System.Security.Principal;

namespace Profile.UI.Identity
{
    public interface ICurrentUserFactory
    {
        AppUser CreateCurrentUser(IPrincipal principal);
    }
}