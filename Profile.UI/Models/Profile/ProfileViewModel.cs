using System.Collections.Generic;
using Profile.UI.Models.Report;

namespace Profile.UI.Models.Profile
{
    public class ProfileViewModel : ProfileMainInfoViewModel
    {
        public int Id { get; set; }

        public ContactsViewModel Contacts { get; set; }
        public PositionViewModel Position { get; set; }
        public JobViewModel Job { get; set; }
        public QualificationViewModel Qualification { get; set; } 

        public ArtefactsViewModel Artefacts { get; set; }
        public List<IssueViewModel> Issues { get; set; }
    }
}