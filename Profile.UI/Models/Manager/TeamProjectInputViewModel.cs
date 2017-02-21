using System.Collections.Generic;
using Profile.UI.ModelEnums;
using Profile.UI.Models.Mentor;

namespace Profile.UI.Models.Manager
{
    public class TeamProjectInputViewModel
    {
        public TeamProjectInputViewModel()
        {
            Specializations = new List<SpecializationNameViewModel>();
            ScrumMasters = new List<ScrumHasProjectViewModel>();
            TraineesForSelect = new List<TraineeViewModel>();
            MentorsForSelect = new List<UserBySpecialization>();
        }

        public List<TraineeViewModel> TraineesForSelect { get; set; }
        public List<UserBySpecialization> MentorsForSelect { get; set; }
        public List<SpecializationNameViewModel> Specializations { get; set; }
        public List<ScrumHasProjectViewModel> ScrumMasters { get; set; }
        public ProjectViewModel Project { get; set; }
        public GroupWithTraineesViewModel Group { get; set; }
        public ManagerFormType FormType { get; set; }
    }
}