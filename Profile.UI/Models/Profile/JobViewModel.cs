using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Profile.UI.Models.Profile
{
    public class JobViewModel
    {
        public string CurrentPosition { get; set; }
        public string CurrentWork { get; set; }
        public string EmploymentDuration { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string JobDuties { get; set; }
    }
}