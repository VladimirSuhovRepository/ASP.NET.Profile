using System.Reflection;
using System.Web.Mvc;
using log4net;

namespace Profile.UI.Controllers
{
    public class ArtefactController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ArtefactController()
        {
        }

        [HttpGet]
        public PartialViewResult Artefacts(int profileId)
        {
            return PartialView("_PartialArtefacts");
        }

        [HttpGet]
        public PartialViewResult EditArtefacts(int profileId)
        {
            return PartialView("_EditPartialArtefacts");
        }
    }
}