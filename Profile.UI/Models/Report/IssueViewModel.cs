using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Profile.UI.Models.Report
{
    public class IssueViewModel
    {
        public string Key { get; set; }
        public string Summary { get; set; }
        public int OriginalEstimate { get; set; }
        public int TimeSpent { get; set; }
        public string IssueType { get; set; }
        public string IconUrl { get; set; }
        public string Status { get; set; }
        public string ColorName { get; set; }
    }
}