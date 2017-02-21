using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Profile.UI.Models
{
    public class TraineeViewModel
    {
        public int Id { get; set; }
        public int MentorId { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public string Project { get; set; }
        public string ReleaseDuration { get; set; }
        public string Command { get; set; }
        public bool IsAllowed { get; set; }
        public bool IsReviewed { get; set; }
    }
}