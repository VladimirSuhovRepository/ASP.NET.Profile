using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Profile.UI.Models
{
    public class MetricsViewModel
    {
        public int TraineesCount { get; set; }
        public int ProjectsCount { get; set; }
        public string ProjectsLabel { get; set; }
        public string TraineesLabel { get; set; }
    }
}