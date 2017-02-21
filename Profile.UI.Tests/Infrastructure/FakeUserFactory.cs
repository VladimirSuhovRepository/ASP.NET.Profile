using System.Security.Principal;
using Profile.UI.Identity;

namespace Profile.UI.Tests.Infrastructure
{
    internal class FakeUserFactory : ICurrentUserFactory
    {
        private readonly FakeUser _fakeUser;

        public FakeUserFactory()
        {
            _fakeUser = new FakeUser();
        }

        public FakeUserFactory(FakeUser fakeUser)
        {
            _fakeUser = fakeUser;
        }

        public AppUser CreateCurrentUser(IPrincipal principal)
        {
            return _fakeUser;
        }
    }
}
