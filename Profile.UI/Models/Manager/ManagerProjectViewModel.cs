using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Profile.UI.Models.Manager
{
    public class ManagerProjectViewModel : ProjectViewModel
    {
        public ManagerProjectViewModel()
        {
            Groups = new List<GroupViewModel>();
        }

        public List<GroupViewModel> Groups { get; set; }
    }
}