using System;
using System.Collections.Generic;

namespace Profile.DAL.Entities
{
    public class Group
    {
        public Group()
        {
            Trainees = new HashSet<Trainee>();
        }

        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string TeamPurpose { get; set; }
        public string TeamworkDescription { get; set; }

        public int? ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public virtual ICollection<Trainee> Trainees { get; set; }

        public int? ScrumMasterId { get; set; }
        public virtual ScrumMaster ScrumMaster { get; set; }

        public bool HasScrumMaster => ScrumMasterId.HasValue;
    }
}
