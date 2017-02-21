using System.Collections.Generic;

namespace Profile.UI.Models.Project
{
    public class ProjectDescriptionViewModel : ProjectViewModel
    {
        public ProjectDescriptionViewModel()
        {
            Groups = new List<GroupDescriptionViewModel>();
        }

        public bool IsCurrentUserOwner { get; set; }
        public IList<GroupDescriptionViewModel> Groups { get; set; }
    }
}