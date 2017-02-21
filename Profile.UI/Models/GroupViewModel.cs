using System;

namespace Profile.UI.Models
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string TeamPurpose { get; set; }
        public string TeamWorkDescription { get; set; }
        public int ScrumMasterId { get; set; }
        public int ProjectId { get; set; }
    }
}