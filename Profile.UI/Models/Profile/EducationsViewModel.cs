using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Profile.UI.Models.Profile
{
    public class EducationsViewModel
    {
        public List<CourseViewModel> Courses { get; set; }
        public List<UniversityViewModel> Universities { get; set; }
    }
}