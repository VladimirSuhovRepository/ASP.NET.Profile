using System;
using System.Collections.Generic;
using System.Linq;

namespace Profile.DAL.Entities
{
    public class Project
    {
        public Project()
        {
            Groups = new List<Group>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FullDescription { get; set; }
        public string ShortDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public bool IsInArchive { get; set; }

        public ProjectStatus Status
        {
            get { return GetStatus(); }

            // Setter is needed to store status property into DB. 
            // It enables us to use the property in LinqToEntities queries
            private set { }
        }

        public virtual ICollection<Group> Groups { get; set; }

        public bool IsScrumMasterOwner(int userId)
        {
            return Groups.Any(g => g.ScrumMasterId == userId);
        }

        private ProjectStatus GetStatus()
        {
            if (IsInArchive) return ProjectStatus.InArchive;

            if (Groups.Count == 1 && Groups.Single().ScrumMaster == null)
            {
                return ProjectStatus.Created;
            } 

            if (string.IsNullOrEmpty(ShortDescription) ||
                string.IsNullOrEmpty(FullDescription))
            {
                return ProjectStatus.WaitingForDescription;
            }

            return ProjectStatus.InProgress;
        }
    }
}
