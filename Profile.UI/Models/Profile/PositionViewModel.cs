using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Profile.UI.Models.Profile
{
    public class PositionViewModel
    {
        public string DesirablePosition { get; set; }
        public string ExperienceAtITLab { get; set; }

        [DataType(DataType.MultilineText)]
        public string ProfessionalPurposes { get; set; }
    }
}