using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.DAL.Entities
{
    public class TraineeProfile
    {
        public TraineeProfile()
        {
            Languages = new List<Language>();
            Universities = new List<University>();
            Courses = new List<Course>();
            MainSkills = new List<MainSkill>();
            SoftSkills = new List<SoftSkill>();
            Files = new List<File>();
            Links = new List<Link>();
        }

        [Key]
        [ForeignKey("Trainee")]
        public int Id { get; set; }

        // Expectations
        public string DesirablePosition { get; set; }
        public string ExperienceAtITLab { get; set; }
        public string ProfessionalPurposes { get; set; }

        // Experience
        public string CurrentPosition { get; set; }
        public string CurrentWork { get; set; }
        public string EmploymentDuration { get; set; }
        public string JobDuties { get; set; }

        // Qualification
        public string Strengths { get; set; }
        public string Weaknesses { get; set; }

        // Additional
        public virtual ICollection<Language> Languages { get; set; }
        public virtual ICollection<University> Universities { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<MainSkill> MainSkills { get; set; }
        public virtual ICollection<SoftSkill> SoftSkills { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Link> Links { get; set; }

        public int? TraineeId { get; set; }
        public virtual Trainee Trainee { get; set; }
    }
}
