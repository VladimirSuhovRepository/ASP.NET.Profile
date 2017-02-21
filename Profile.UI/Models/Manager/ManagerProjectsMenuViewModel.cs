using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Profile.UI.Models.Manager
{
    public class ManagerProjectsMenuViewModel
    {
        public ManagerProjectsMenuViewModel()
        {
            Projects = new List<ManagerProjectViewModel>();
            ActiveTeamId = 0;
        }

        public ManagerProjectsMenuViewModel(int activeTeamId, List<ManagerProjectViewModel> projects)
        {
            Projects = projects;
            ActiveTeamId = activeTeamId;
        }

        public List<ManagerProjectViewModel> Projects { get; set; }
        public int ActiveTeamId { get; set; }
    }
}