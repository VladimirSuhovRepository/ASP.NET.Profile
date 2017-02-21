using System.Collections.Generic;

namespace Profile.UI.Models.Profile
{
    public class SpecializationViewModel
    {
        public SpecializationViewModel()
        {
            MainSkills = new List<MainSkillViewModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<MainSkillViewModel> MainSkills { get; set; }
    }
}