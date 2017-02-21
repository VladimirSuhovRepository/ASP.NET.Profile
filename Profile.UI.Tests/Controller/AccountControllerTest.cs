using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Profile.UI.Controllers;
using Profile.UI.Models.Json;
using Profile.UI.Tests.Ninject;

namespace Profile.UI.Tests.Controller
{
    [TestClass]
    public class AccountControllerTest : DependencyInjectedTest
    {
        private readonly AccountController _controller;

        public AccountControllerTest()
        {
            NewScope();
            _controller = Kernel.Get<AccountController>();
        }

        [TestMethod]
        public void GetDefaultAvatarActionReturnedNotNull()
        {
            var result = _controller.GetDefaultAvatarUrl();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDefaultAvatarActionReturnedDefaultAvatarUrl()
        {
            string defaultAvatarUrl = @"/images/avatar-small-placeholder.png";
            var result = _controller.GetDefaultAvatarUrl() as JsonResult;
            dynamic jsonData = result.Data;

            Assert.AreEqual(defaultAvatarUrl, jsonData.DefaultUrl);
        }

        [TestMethod]
        public async Task ForgotPasswordActionReturnedFalseWhenEmailDoesNotExist()
        {
            string email = "login@mail.com";
            var result = await _controller.ForgotPassword(email);
            dynamic jsonData = result.Data;

            Assert.IsFalse(jsonData.IsEmailExisting);
        }

        [TestMethod]
        public void ResetPasswordActionReturnedView()
        {
            string fakeToken = "123";
            var result = _controller.ResetPassword(fakeToken) as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ResetPasswordActionReturnedViewWithViewBag()
        {
            string fakeToken = "123";
            var result = _controller.ResetPassword(fakeToken) as ViewResult;

            Assert.AreEqual(result.ViewBag.Token, fakeToken);
        }

        [TestMethod]
        public async Task ResetPasswordActionReturnedJson()
        {
            var request = new ResetPasswordRequestJsonModel
            {
                Password = "QwertY13",
                ConfirmPassword = "QwertY13",
                Token = "123"
            };

            var result = await _controller.ResetPassword(request);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ResetPasswordActionReturnedNonSuccessed()
        {
            var request = new ResetPasswordRequestJsonModel
            {
                Password = "QwertY13",
                ConfirmPassword = "QwertY13",
                Token = "123"
            };

            var result = await _controller.ResetPassword(request);
            dynamic jsonData = result.Data;

            Assert.IsFalse(jsonData.Succeeded);
        }
    }
}
