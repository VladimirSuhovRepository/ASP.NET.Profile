using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Profile.UI.HtmlHelpers;
using Profile.UI.Tests.Infrastructure;
using Profile.UI.Tests.Ninject;

namespace Profile.UI.Tests.HtmlHelpers
{
    [TestClass]
    public class HtmlHelperTests : DependencyInjectedTest
    {
        private const string AppPathModifier = MvcHelper.AppPathModifier;

        [TestMethod]
        public void IsCurrentRouteExtension_ReturnedTrue_WhenRoutesAreSame()
        {
            int id = 1;
            var htmlHelper = MvcHelper.GetHtmlHelper("Home", "Index", new { Id = id });
            bool result = htmlHelper.IsCurrentRoute("Home", "Index", new { Id = id });

            Assert.IsTrue(result, "Routes are not same");
        }

        [TestMethod]
        public void IsCurrentRouteExtension_ReturnedFalse_WhenRoutesAreNotSame()
        {
            var htmlHelper = MvcHelper.GetHtmlHelper("Home", "Index");
            bool result = htmlHelper.IsCurrentRoute("Home", "About");

            Assert.IsFalse(result, "Routes are same");
        }

        [TestMethod]
        public void IsCurrentRouteExtension_ReturnedFalse_WhenIdsAreNotSame()
        {
            var htmlHelper = MvcHelper.GetHtmlHelper("Home", "Index", new { Id = 1 });
            bool result = htmlHelper.IsCurrentRoute("Home", "Index", new { Id = 2 });

            Assert.IsFalse(result, "Routes are same");
        }

        [TestMethod]
        public void IsCurrentRouteExtension_ReturnedFalse_WhenRouteHasId_AndLinkDoNotHave()
        {
            var htmlHelper = MvcHelper.GetHtmlHelper("Home", "Index", new { Id = 1 });
            bool result = htmlHelper.IsCurrentRoute("Home", "Index");

            Assert.IsFalse(result, "Routes are same");
        }

        [TestMethod]
        public void IsCurrentRouteExtension_ReturnedFalse_WhenRoute_DoNotHaveId_AndLinkHas()
        {
            var htmlHelper = MvcHelper.GetHtmlHelper("Home", "Index");
            bool result = htmlHelper.IsCurrentRoute("Home", "Index", new { Id = 1 });

            Assert.IsFalse(result, "Routes are same");
        }

        [TestMethod]
        public void MenuItemExtension_ReturnedCorrectHtml()
        {
            string actionName = "View";
            string controllerName = "Profile";
            int id = 12;
            string linkText = "Профиль";

            StringBuilder exceptedHtml = new StringBuilder();

            exceptedHtml.Append("<li class=''>");
            exceptedHtml.Append($@"<a href=""{AppPathModifier}/{controllerName}/{actionName}/{id}"">");
            exceptedHtml.Append($"{linkText}</a></li>");

            var htmlHelper = MvcHelper.GetHtmlHelper();
            var actualHtml = htmlHelper.MenuItem(linkText, actionName, controllerName, new { Id = id }, null);

            Assert.AreEqual(exceptedHtml.ToString(), actualHtml.ToHtmlString(), "Html is not correct");
        }

        [TestMethod]
        public void MenuItemExtension_ElementHasActiveClass_WhenActionLinksToCurrentPage()
        {
            string actionName = "View";
            string controllerName = "Profile";
            string exceptedElementHtml = "<li class='active'>";

            var htmlHelper = MvcHelper.GetHtmlHelper(controllerName, actionName);
            var actualHtml = htmlHelper.MenuItem("Профиль", actionName, controllerName);
            bool hasActiveClass = actualHtml.ToHtmlString().Contains(exceptedElementHtml);

            Assert.IsTrue(hasActiveClass, "Element does not contain active class");
        }

        [TestMethod]
        public void MenuItemExtension_ElementDoesNotHave_ActiveClass_WhenActionDoNotLink_ToCurrentPage()
        {
            string exceptedElementHtml = "<li class='active'>";

            var htmlHelper = MvcHelper.GetHtmlHelper();
            var actualHtml = htmlHelper.MenuItem("Профиль", "View", "Profile");
            bool hasActiveClass = actualHtml.ToHtmlString().Contains(exceptedElementHtml);

            Assert.IsFalse(hasActiveClass, "Element does not contain active class");
        }

        [TestMethod]
        public void MenuItemExtension_ReturnedCorrectHtml_WhenIsEnabledParam_IsFalse()
        {
            string linkText = "Профиль";
            string exceptedHtml = $@"<li class=""disabled""><a>{linkText}</a></li>";

            var htmlHelper = MvcHelper.GetHtmlHelper();
            var actualHtml = htmlHelper.MenuItem(linkText, "View", "Profile", false);

            Assert.AreEqual(exceptedHtml, actualHtml.ToHtmlString(), "Html is not correct");
        }
    }
}
