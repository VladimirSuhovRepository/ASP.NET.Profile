using System.Reflection;
using System.Web.Mvc;
using log4net;
using Profile.BL.Interfaces;
using Profile.DAL.Entities;
using Profile.UI.Filters;
using Profile.UI.Identity;
using Profile.UI.Mappers;
using Profile.UI.Models.Json;

namespace Profile.UI.Controllers
{
    [PermissionAuthorize(Permission = PermissionType.OnlyTrainee)]
    public class ProfileController : BaseController
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IProfileProvider _profileProvider;
        private ProfileMapper _profileMapper;

        public ProfileController(
            IProfileProvider profileProvider, 
            ProfileMapper profileMapper,
            ICurrentUserFactory userFactory)
            : base(userFactory)
        {
            _profileProvider = profileProvider;
            _profileMapper = profileMapper;
        }

        [HttpGet]
        [OverrideAuthorization]
        [PermissionAuthorize]
        public ActionResult View(int? id)
        {
            int traineeId = id.HasValue ? id.Value : CurrentUser.Id.Value;
            var profile = _profileProvider.GetProfileByTraineeId(traineeId);
            double traineeRaiting = _profileProvider.GetTraineeRating(traineeId);

            var profileViewModel = _profileMapper.ToProfileViewModel(profile, traineeRaiting);

            return View(profileViewModel);
        }

        [HttpGet]
        public ActionResult Edit()
        {
            int id = CurrentUser.Id.Value;
            var profile = _profileProvider.GetProfileByTraineeId(id);
            double traineeRaiting = _profileProvider.GetTraineeRating(id);

            var profileViewModel = _profileMapper.ToProfileViewModel(profile, traineeRaiting);

            return View(profileViewModel);
        }

        [HttpPost]
        public JsonResult EditMainInfo(ProfileMainInfoJson profileMainInfoJson)
        {
            profileMainInfoJson.TrimAndUppercaseFirst();

            TraineeProfile editingProfileWithMainInfo = _profileMapper.FromProfileMainInfoJsonModel(profileMainInfoJson);
            TraineeProfile editedProfile = _profileProvider.UpdateMainInfo(editingProfileWithMainInfo);
            ProfileMainInfoJson updatedProfileMainInfoJson = _profileMapper.ToJsonProfileMainInfoModel(editedProfile);

            return Json(updatedProfileMainInfoJson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditQualification(ProfileQualificationJson profileQualificationJson)
        {
            profileQualificationJson.TrimAndUppercaseFirst();

            var updatingProfileWithQualification = _profileMapper.FromProfileQualificationJsonModel(profileQualificationJson);

            var updatedProfile = _profileProvider.UpdateQualification(updatingProfileWithQualification);

            var updatedProfileMainInfoJson = _profileMapper.ToProfileQualificationJsonModel(updatedProfile);

            return Json(updatedProfileMainInfoJson);
        }
    }
}
