using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using log4net;
using Profile.BL.Interfaces;
using Profile.UI.Mappers;
using Profile.UI.Models.Json;

namespace Profile.UI.Controllers
{
    public class LanguageController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private ILanguageProvider _languageProvider;
        private LanguageMapper _languageMapper;

        public LanguageController(ILanguageProvider languageProvider, LanguageMapper languageMapper)
        {
            _languageProvider = languageProvider;
            _languageMapper = languageMapper;
        }

        [HttpGet]
        public PartialViewResult Languages(int profileId)
        {
            var languages = _languageProvider.GetLanguagesByProfileId(profileId);

            var languagesViewModel = languages.Select(_languageMapper.ToViewModel).ToList();

            return PartialView("_PartialLanguages", languagesViewModel);
        }

        [HttpGet]
        public PartialViewResult EditLanguages(int profileId)
        {
            var languages = _languageProvider.GetLanguagesByProfileId(profileId);

            var languagesViewModel = languages.Select(_languageMapper.ToViewModel).ToList();

            return PartialView("_EditPartialLanguages", languagesViewModel);
        }

        [HttpPost]
        public JsonResult EditLanguages(ProfileLanguagesJson jsonProfileLanguages)
        {
            jsonProfileLanguages.TrimAndUppercaseFirst();

            var updatingLanguages = _languageMapper.FromProfileLanguagesJsonModel(jsonProfileLanguages.ProfileId, jsonProfileLanguages);

            var updatedLanguages = _languageProvider.UpdateLanguages(updatingLanguages);

            ProfileLanguagesJson updatedLanguagesJson = _languageMapper.ToProfileLanguagesJsonModel(jsonProfileLanguages.ProfileId, updatedLanguages);

            return Json(updatedLanguagesJson, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public void RemoveLanguage(int languageId)
        {
            _languageProvider.RemoveLanguage(languageId);
        }
    }
}