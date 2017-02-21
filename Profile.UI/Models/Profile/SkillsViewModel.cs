using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Profile.UI.Models.Profile
{
    public class SkillsViewModel
    {
        public SpecializationViewModel Specialization { get; set; }
        public List<SoftSkillViewModel> SoftSkills { get; set; }
    }
}