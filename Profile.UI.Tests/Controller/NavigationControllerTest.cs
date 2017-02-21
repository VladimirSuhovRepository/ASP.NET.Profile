using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Profile.DAL.Entities;
using Profile.UI.Controllers;
using Profile.UI.Infrastructure;
using Profile.UI.Models.Profile;
using Profile.UI.Tests.Infrastructure;
using Profile.UI.Tests.Ninject;

namespace Profile.UI.Tests.Controller
{
    [TestClass]
    public class NavigationControllerTest : DependencyInjectedTest
    {
        private NavigationController _controller;

        [TestMethod]
        public void GetHeaderNavigationAction_Returned_CorrectPartialView_WhenUserIsManager()
        {
            string exceptedViewName = "Manager/_HeaderNavigation";

            TestHeaderForRole(RoleType.Manager, exceptedViewName);
        }

        [TestMethod]
        public void GetHeaderNavigationAction_Returned_CorrectPartialView_WhenUserIsHR()
        {
            string exceptedViewName = "HR/_HeaderNavigation";

            TestHeaderForRole(RoleType.HR, exceptedViewName);
        }

        [TestMethod]
        public void GetHeaderNavigationAction_Returned_CorrectPartialView_WhenUserIsAdmin()
        {
            string exceptedViewName = "Admin/_HeaderNavigation";

            TestHeaderForRole(RoleType.Admin, exceptedViewName);
        }

        [TestMethod]
        public void GetHeaderNavigationAction_Returned_CorrectPartialView_WhenUserIsMentor()
        {
            string exceptedViewName = "Mentor/_HeaderNavigation";

            TestHeaderForRole(RoleType.Mentor, exceptedViewName);
        }

        [TestMethod]
        public void GetHeaderNavigationAction_Returned_CorrectPartialView_WhenUserIsScrum()
        {
            string exceptedViewName = "ScrumMaster/_HeaderNavigation";

            TestHeaderForRole(RoleType.ScrumMaster, exceptedViewName);
        }

        [TestMethod]
        public void GetHeaderNavigationAction_Returned_CorrectPartialView_WhenUserIsTrainee()
        {
            string exceptedViewName = "Trainee/_HeaderNavigation";

            TestHeaderForRole(RoleType.Trainee, exceptedViewName);
        }

        [TestMethod]
        public void GetHeaderNavigationAction_Returned_TwoPartialViews_WhenUserIsScrumMentor()
        {
            var fakeUser = new FakeUser(1);
            string exceptedFirstViewName = "ScrumMaster/_HeaderNavigation";
            string exceptedSecondViewName = "Mentor/_HeaderNavigation";

            fakeUser.AddRole(RoleType.Mentor);
            fakeUser.AddRole(RoleType.ScrumMaster);
            _controller = CreateController(fakeUser);

            var result = _controller.GetHeaderNavigation() as MultiplePartialViewResult;

            Assert.IsNotNull(result, "It is not a MultiplePartialViewResult");
            Assert.AreEqual(2, result.PartialViews.Count, "There is not two views");

            var firstView = result.PartialViews[0];
            var secondView = result.PartialViews[1];

            Assert.AreEqual(exceptedFirstViewName, firstView.ViewName, "There is no SM navigation menu");
            Assert.AreEqual(exceptedSecondViewName, secondView.ViewName, "There is no mentor navigation menu");
        }

        [TestMethod]
        public void GetHeaderNavigationAction_Returned_CorrectPartialView_WhenUserIs_OnManagerPages()
        {
            string exceptedViewName = "Manager/_CustomManagerNavigation";
            var fakeUser = new FakeUser(1);

            fakeUser.AddRole(RoleType.Manager);
            _controller = CreateController(fakeUser, "ManageRoles", "Manager");

            var result = _controller.GetHeaderNavigation() as PartialViewResult;

            Assert.IsNotNull(result, "It is not a PartialViewResult");
            Assert.AreEqual(exceptedViewName, result.ViewName, "Name does not match");
        }

        [TestMethod]
        public void GetHeader_Returned_DefaultHeader_WhenUserRequests_AccountController()
        {
            string exceptedViewName = "_DefaultHeader";
            var fakeUser = new FakeUser(1);

            _controller = CreateController(fakeUser, "Login", "Account");

            var result = _controller.GetHeader() as PartialViewResult;

            Assert.IsNotNull(result, "It is not a partial view");
            Assert.AreEqual(exceptedViewName, result.ViewName, $"It is not a {exceptedViewName} view");
        }

        [TestMethod]
        public void GetHeader_Returned_GuestHeader_WhenUser_IsNotLoggedIn()
        {
            string exceptedViewName = "_GuestHeader";
            var fakeUser = new FakeUser();

            _controller = CreateController(fakeUser);

            var result = _controller.GetHeader() as PartialViewResult;

            Assert.IsNotNull(result, "It is not a partial view");
            Assert.AreEqual(exceptedViewName, result.ViewName, $"It is not a {exceptedViewName} view");
        }

        [TestMethod]
        public void GetHeader_Returned_UserHeader_WhenUser_IsLoggedIn()
        {
            string exceptedViewName = "_UserHeader";
            var fakeUser = new FakeUser(1);

            _controller = CreateController(fakeUser);

            var result = _controller.GetHeader() as PartialViewResult;
            bool isUserTrainee = fakeUser.HasRole(RoleType.Trainee);

            Assert.IsNotNull(result, "It is not a partial view");
            Assert.AreEqual(exceptedViewName, result.ViewName, $"It is not a {exceptedViewName} view");

            var viewBagIsUserTrainee = result.ViewBag.IsUserTrainee;

            Assert.IsNotNull(viewBagIsUserTrainee, "View bag does not contain IsUserTrainee property");

            Assert.AreEqual(isUserTrainee,
                            viewBagIsUserTrainee, 
                            "ViewBag.IsUserTrainee contains incorrect data");
        }

        [TestMethod]
        public void GetHeader_ViewBag_IsUserTrainee_ContainsFalse_WhenUser_IsNotTrainee()
        {
            var fakeUser = new FakeUser(1);

            _controller = CreateController(fakeUser);

            var result = _controller.GetHeader() as PartialViewResult;

            Assert.IsNotNull(result, "It is not a partial view");

            var viewBagIsUserTrainee = result.ViewBag.IsUserTrainee;

            Assert.IsNotNull(viewBagIsUserTrainee, "View bag does not contain IsUserTrainee property");
            Assert.IsFalse(viewBagIsUserTrainee, "ViewBag.IsUserTrainee contains incorrect data");
        }

        [TestMethod]
        public void GetHeader_ViewBag_IsUserTrainee_ContainsTrue_WhenUser_IsTrainee()
        {
            var fakeUser = new FakeUser(1);

            fakeUser.AddRole(RoleType.Trainee);
            _controller = CreateController(fakeUser);

            var result = _controller.GetHeader() as PartialViewResult;

            Assert.IsNotNull(result, "It is not a partial view");

            var viewBagIsUserTrainee = result.ViewBag.IsUserTrainee;

            Assert.IsNotNull(viewBagIsUserTrainee, "View bag does not contain IsUserTrainee property");
            Assert.IsTrue(viewBagIsUserTrainee, "ViewBag.IsUserTrainee contains incorrect data");
        }

        [TestMethod]
        public void GetTraineeProfileNavigationMenuAction_Returned_OwnerNavigationMenu_WhenUserIsTrainee_AndProfileOwner()
        {
            int userId = 1;
            string exceptedViewName = "Trainee/_ProfileOwnerNavigation";
            var fakeUser = new FakeUser(userId);

            fakeUser.AddRole(RoleType.Trainee);
            _controller = CreateController(fakeUser);

            var viewModel = new ProfileMainInfoViewModel
            {
                TraineeId = userId
            };

            var result = _controller.GetTraineeProfileNavigationMenu(viewModel) as PartialViewResult;

            Assert.IsNotNull(result, "It is not a partial view");
            Assert.AreEqual(exceptedViewName, result.ViewName, $"It is not a {exceptedViewName} view");

            var resultViewModel = result.Model as ProfileMainInfoViewModel;

            Assert.IsNotNull(resultViewModel, "View does not contain a model of ProfileMainInfoViewModel type");
        }

        [TestMethod]
        public void GetTraineeProfileNavigationMenuAction_Returned_CommonNavigationMenu_WhenUserIsNotProfileOwner()
        {
            string exceptedViewName = "Trainee/_ProfileNavigation";
            var fakeUser = new FakeUser(1);

            _controller = CreateController(fakeUser);

            var viewModel = new ProfileMainInfoViewModel
            {
                TraineeId = 2
            };

            var result = _controller.GetTraineeProfileNavigationMenu(viewModel) as PartialViewResult;

            Assert.IsNotNull(result, "It is not a partial view");
            Assert.AreEqual(exceptedViewName, result.ViewName, $"It is not a {exceptedViewName} view");

            var resultViewModel = result.Model as ProfileMainInfoViewModel;

            Assert.IsNotNull(resultViewModel, "View does not contain a model of ProfileMainInfoViewModel type");
        }

        private NavigationController CreateController(FakeUser fakeUser)
        {
            return CreateController(fakeUser, "Index", "Home");
        }

        private NavigationController CreateController(FakeUser fakeUser,
                                                      string actionName,
                                                      string controllerName)
        {
            var fakeFactory = new FakeUserFactory(fakeUser);
            var controller = new NavigationController(fakeFactory);

            controller.ControllerContext = MvcHelper.GetControllerContext(actionName,
                                                                          controllerName,
                                                                          controller);

            return controller;
        }

        private void TestHeaderForRole(RoleType role, string exceptedViewName)
        {
            var fakeUser = new FakeUser(1);

            fakeUser.AddRole(role);
            _controller = CreateController(fakeUser);

            var result = _controller.GetHeaderNavigation() as MultiplePartialViewResult;

            Assert.IsNotNull(result, "It is not a MultiplePartialViewResult");
            Assert.AreEqual(1, result.PartialViews.Count, "There is not one view");

            var view = result.PartialViews.Single();

            Assert.AreEqual(exceptedViewName, view.ViewName, "Name does not match");
        }
    }
}
