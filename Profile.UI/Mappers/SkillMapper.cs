using System.Collections.Generic;
using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.Models;
using Profile.UI.Models.Json;
using Profile.UI.Models.Profile;

namespace Profile.UI.Mappers
{
    public class SkillMapper
    {
        private IMapper _mapper;

        public SkillMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public SoftSkill FromSoftSkillJsonModel(SoftSkillJson softSkillJson)
        {
            var softSkill = _mapper.Map<SoftSkillJson, SoftSkill>(softSkillJson);

            return softSkill;
        }

        public SoftSkillJson ToSoftSkillJsonModel(SoftSkill softSkill)
        {
            var softSkillJson = _mapper.Map<SoftSkill, SoftSkillJson>(softSkill);

            return softSkillJson;
        }

        public SoftSkill FromSoftSkillViewModel(int profileId, SoftSkillJson softSkillJson)
        {
            var softSkill = _mapper.Map<SoftSkillJson, SoftSkill>(softSkillJson);

            softSkill.TraineeProfileId = profileId;

            return softSkill;
        }

        public SkillsViewModel ToSkillViewModel(SpecializationViewModel specializationViewModel, List<SoftSkillViewModel> softSkillsViewModel)
        {
            var skillsViewModel = new SkillsViewModel
            {
                Specialization = specializationViewModel,
                SoftSkills = softSkillsViewModel
            };

            return skillsViewModel;
        }

        public SoftSkillViewModel ToSoftSkillViewModel(SoftSkill softSkill)
        {
            var softSkillViewModel = _mapper.Map<SoftSkill, SoftSkillViewModel>(softSkill);

            return softSkillViewModel;
        }

        public MainSkillViewModel ToMainSkillViewModel(MainSkill mainSkill, bool selected = false)
        {
            var mainSkillViewModel = _mapper.Map<MainSkill, MainSkillViewModel>(mainSkill);

            mainSkillViewModel.Selected = selected;

            return mainSkillViewModel;
        }

        public SpecializationViewModel ToSpecializationViewModel(List<MainSkillViewModel> mainSkillsForView, MainSkill firstMainSkill)
        {
            var specializationViewModel = new SpecializationViewModel
            {
                MainSkills = mainSkillsForView,
                Id = firstMainSkill != null ? firstMainSkill.Specialization.Id : 0,
                Name = firstMainSkill != null ? firstMainSkill.Specialization.Name : string.Empty
            };

            return specializationViewModel;
        }

        public SkillViewModel ToSkillViewModel(Skill ability)
        {
            var abilityViewModel = _mapper.Map<Skill, SkillViewModel>(ability);

            return abilityViewModel;
        }
    }
}