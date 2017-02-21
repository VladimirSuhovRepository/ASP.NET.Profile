using System.Web.Mvc;
using Profile.UI.Identity;

namespace Profile.UI.Controllers
{
    public abstract class BaseController : Controller
    {
        private ICurrentUserFactory _userFactory;

        protected BaseController(ICurrentUserFactory userFactory)
        {
            _userFactory = userFactory;
        }

        public AppUser CurrentUser => _userFactory.CreateCurrentUser(User);
    }
}