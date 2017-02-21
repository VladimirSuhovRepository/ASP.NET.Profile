using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Profile.UI.Models.Profile
{
    public class QualificationViewModel
    {
        [DataType(DataType.MultilineText)]
        public string Strengths { get; set; }

        [DataType(DataType.MultilineText)]
        public string Weaknesses { get; set; }
    }
}