using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Profile.UI.Models
{
    public class ProjectTraineeViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public bool IsAllowed { get; set; }
        public bool IsReviewed { get; set; }
        public ProjectViewModel Project { get; set; }
    }
}