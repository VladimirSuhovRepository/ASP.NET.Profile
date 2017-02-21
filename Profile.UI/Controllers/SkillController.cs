using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using log4net;
using Profile.BL.Interfaces;
using Profile.DAL.Entities;
using Profile.UI.Mappers;
using Profile.UI.Models.Json;
using Profile.UI.Models.Profile;

namespace Profile.UI.Controllers
{
    public class SkillController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IMainSkillProvider _mainSkillProvider;
        private ISoftSkillProvider _softSkillProvider;
        private SkillMapper _skillMapper;

        public SkillController(IMainSkillProvider mainSkillProvider, ISoftSkillProvider softSkillProvider, SkillMapper skillMapper)
        {
            _mainSkillProvider = mainSkillProvider;
            _softSkillProvider = softSkillProvider;
            _skillMapper = skillMapper;
        }

        #region Skill

        [HttpGet]
        public PartialViewResult Skills(int profileId)
        {
            SkillsViewModel skills = _skillMapper.ToSkillViewModel(GetSpecializationWithAllSkills(profileId), GetSoftSkills(profileId));

            return PartialView("_EditPartialSkills", skills);
        }

        [HttpPost]
        public JsonResult EditSkills(SkillsContainerJson jsonSkillContainer)
        {
            jsonSkillContainer.TrimAndUppercaseFirst();

            List<int> mainSkillsId = jsonSkillContainer.Specializations.First().IdSkills;
            _mainSkillProvider.UpdateSelectedMainSkills(jsonSkillContainer.ProfileId, mainSkillsId);

            List<SoftSkill> updatingSoftSkills = jsonSkillContainer.SoftSkills
                .Select(skill => _skillMapper.FromSoftSkillViewModel(jsonSkillContainer.ProfileId, skill))
                .ToList();

            List<SoftSkill> updatedSoftSkills = _softSkillProvider.UpdateSoftSkills(updatingSoftSkills);

            jsonSkillContainer.SoftSkills = updatedSoftSkills.Select(_skillMapper.ToSoftSkillJsonModel).ToList();

            return Json(jsonSkillContainer, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region MainSkill

        [HttpGet]
        public PartialViewResult MainSkills(int profileId)
        {
            var specialization = GetSpecializationWithSelectedSkills(profileId);

            return PartialView("_PartialMainSkills", specialization);
        }

        #endregion

        #region SoftSkill

        [HttpGet]
        public PartialViewResult SoftSkills(int profileId)
        {
            var softSkills = GetSoftSkills(profileId);

            return PartialView("_PartialSoftSkills", softSkills);
        }

        [HttpDelete]
        public void RemoveSoftSkill(int idSoftSkill)
        {
            _softSkillProvider.RemoveSoftSkill(idSoftSkill);
        }

        #endregion

        #region Private methods

        private SpecializationViewModel GetSpecializationWithSelectedSkills(int profileId)
        {
            var specialization = GetSpecializationWithAllSkills(profileId);

            specialization.MainSkills = specialization.MainSkills.Where(s => s.Selected).ToList();

            return specialization;
        }

        private SpecializationViewModel GetSpecializationWithAllSkills(int profileId)
        {
            List<MainSkill> allMainSkills = _mainSkillProvider.GetAvailableMainSkills(profileId);
            List<MainSkill> selectedMainSkills = _mainSkillProvider.GetSelectedMainSkills(profileId);

            var mainSkillsForView = allMainSkills
                .Select(s => _skillMapper.ToMainSkillViewModel(s, selectedMainSkills.Contains(s)))
                .ToList();

            var firstMainSkill = selectedMainSkills.FirstOrDefault();
            var specializationForView = _skillMapper.ToSpecializationViewModel(mainSkillsForView, firstMainSkill);

            return specializationForView;
        }

        private List<SoftSkillViewModel> GetSoftSkills(int profileId)
        {
            List<SoftSkill> softSkills = _softSkillProvider.GetSoftSkills(profileId);

            List<SoftSkillViewModel> softSkillsForView = softSkills.Select(_skillMapper.ToSoftSkillViewModel).ToList();

            return softSkillsForView;
        }

        #endregion
    }
}