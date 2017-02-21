using System.Collections.Generic;

namespace Profile.UI.Models.Project
{
    public class GroupDescriptionViewModel : GroupViewModel
    {
        public GroupDescriptionViewModel()
        {
            Trainees = new List<GroupTraineeViewModel>();
        }

        public GroupScrumMasterViewModel ScrumMaster { get; set; }
        public IList<GroupTraineeViewModel> Trainees { get; set; }
    }
}