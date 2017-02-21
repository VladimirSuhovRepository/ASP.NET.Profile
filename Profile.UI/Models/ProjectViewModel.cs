using System;
using Profile.DAL.Entities;

namespace Profile.UI.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string FullDescription { get; set; }
        public string ShortDescription { get; set; }
        public ProjectStatus Status { get; set; }
    }
}