using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Profile.DAL.Entities;
using Profile.UI.Identity;
using Profile.UI.Infrastructure;
using Profile.UI.Models.Profile;
using Profile.UI.Utility;

namespace Profile.UI.Controllers
{
    public class NavigationController : BaseController
    {
        public NavigationController(ICurrentUserFactory userFactory)
            : base(userFactory)
        {
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult GetHeaderNavigation()
        {
            // TODO: It is a temporary solution for custom header in manager's pages
            // Correct it when there is more than one custom header in views
            string requestedController = HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString();

            if (string.Compare(requestedController, "Manager") == 0)
            {
                return PartialView("Manager/_CustomManagerNavigation");
            }

            var multiplePartialViewResult = new MultiplePartialViewResult();

            var roleComparer = new RoleComparer(new Dictionary<RoleType, int>
            {
                { RoleType.ScrumMaster, 1 },
                { RoleType.Mentor, 2 }
            });

            var userRoles = CurrentUser.GetUserRoles().OrderBy(r => r, roleComparer);

            foreach (var role in userRoles)
            {
                multiplePartialViewResult.PartialViews.Add(PartialView($"{role}/_HeaderNavigation"));
            }

            return multiplePartialViewResult;
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult GetHeader()
        {
            // Account controller's views should have a default header
            string requestedController = HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString();

            if (string.Compare(requestedController, "Account") == 0)
            {
                return PartialView("_DefaultHeader");
            }

            if (!CurrentUser.IsAuthenticated) return PartialView("_GuestHeader");

            ViewBag.IsUserTrainee = CurrentUser.HasRole(RoleType.Trainee);

            return PartialView("_UserHeader");
        }

        [ChildActionOnly]
        public ActionResult GetTraineeProfileNavigationMenu(ProfileMainInfoViewModel viewModel)
        {
            if ((CurrentUser.Id == viewModel.TraineeId) && CurrentUser.HasRole(RoleType.Trainee))
            {
                return PartialView("Trainee/_ProfileOwnerNavigation", viewModel);
            }

            return PartialView("Trainee/_ProfileNavigation", viewModel);
        }
    }
}