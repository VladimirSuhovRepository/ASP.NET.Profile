using System.Web.Mvc;

namespace Profile.UI.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult PageNotFound(string error)
        {
            Response.StatusCode = 404;
            Response.ContentType = "text/html";
            ViewData["Description"] = error;

            return View();
        }
    }
}