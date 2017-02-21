using Profile.DAL.Entities;
using Profile.UI.Identity;

namespace Profile.UI.Tests.Infrastructure
{
    public class FakeUser : AppUser
    {
        private readonly int? _id;

        public FakeUser()
        {
        }

        public FakeUser(int id)
        {
            _id = id;
        }

        public override bool IsAuthenticated => _id.HasValue;

        public void AddRole(RoleType role)
        {
            Roles.Add(role);
        }

        protected override int? GetUserId()
        {
            return _id;
        }
    }
}
