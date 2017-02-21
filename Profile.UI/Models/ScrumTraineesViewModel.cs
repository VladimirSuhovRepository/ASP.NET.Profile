using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Profile.UI.Models
{
    public class ScrumTraineesViewModel
    {
        public ScrumTraineesViewModel()
        {
            Reviews = new List<ScrumReviewEditViewModel>();
        }

        public int ScrumMasterId { get; set; }
        public List<ScrumReviewEditViewModel> Reviews { get; set; }
    }
}