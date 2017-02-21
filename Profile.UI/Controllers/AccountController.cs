using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.Owin.Security;
using Profile.BL.Interfaces;
using Profile.UI.Models;
using Profile.UI.Models.Json;

namespace Profile.UI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private IUsersProvider _userProvider;
        private IMapper _mapper;

        public AccountController(IUsersProvider userProvider, IMapper mapper)
        {
            _userProvider = userProvider;
            _mapper = mapper;
        }

        private IAuthenticationManager AuthenticationManager => HttpContext
            .GetOwinContext().Authentication;

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel user)
        {
            string loginFailureErrorMessage = "Введено неправильное имя пользователя / пароль. Попробуйте еще раз.";

            if (!ModelState.IsValid)
            {
                ViewBag.LoginFailture = loginFailureErrorMessage;
                return View();
            }

            // removing white-spaces at the beginning and end of the string in Login property
            user.Login = user.Login.Trim();

            var claim = await _userProvider.AuthenticateAsync(user.Login, user.Password);

            if (claim == null)
            {
                ViewBag.LoginFailture = loginFailureErrorMessage;
                ModelState.Clear();
                return View();
            }

            var authProperties = new AuthenticationProperties { IsPersistent = true };

            if (!user.RememberMe) authProperties.ExpiresUtc = DateTime.UtcNow.AddMinutes(20);

            AuthenticationManager.SignOut();
            AuthenticationManager.SignIn(authProperties, claim);

            return RedirectToAction("GetStartPage", "Home");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public JsonResult GetDefaultAvatarUrl()
        {
            string defaultUrl = _userProvider.GetDefaultAvatarUrl();

            return Json(new { DefaultUrl = defaultUrl });
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ForgotPassword(string email)
        {
            var user = await _userProvider.GetUserByEmailAsync(email);

            if (user != null)
            {
                string callbackUrl = Url.Action("ResetPassword", "Account", null, Request.Url.Scheme);

                await _userProvider.SendRecoverMailToUser(user.Id, callbackUrl);
            }

            return Json(new { IsEmailExisting = user != null });
        }

        [HttpGet]
        public ActionResult ResetPassword(string token)
        {
            ViewBag.Token = token;

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ResetPassword(ResetPasswordRequestJsonModel requestJsonModel)
        {
            var resetPasswordOperationResult = await _userProvider.ResetUserPasswordByToken(
                requestJsonModel.Token,
                requestJsonModel.Password);

            var responseJsonModel = _mapper.Map<ResetPasswordResponseJsonModel>(resetPasswordOperationResult);

            return Json(responseJsonModel);
        }
    }
}
