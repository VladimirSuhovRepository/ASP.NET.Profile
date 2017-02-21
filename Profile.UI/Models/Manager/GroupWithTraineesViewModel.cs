using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Profile.UI.Models.Manager
{
    public class GroupWithTraineesViewModel : GroupViewModel
    {
        public GroupWithTraineesViewModel()
        {
            Trainees = new List<TraineeViewModel>();
        }

        public List<TraineeViewModel> Trainees { get; set; }
    }
}