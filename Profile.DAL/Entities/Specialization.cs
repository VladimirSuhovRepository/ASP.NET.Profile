using System;
using System.Collections.Generic;

namespace Profile.DAL.Entities
{
    public class Specialization
    {
        public const string BA = "BA";
        public const string BackendAndroid = "BE ANDROID";
        public const string BackendJava = "BE JAVA";
        public const string BackendPHP = "BE PHP";
        public const string BackendNET = "BE .NET";
        public const string Design = "UI/UX";
        public const string Frontend = "FD";
        public const string QA = "QA";

        public Specialization()
        {
            Trainees = new List<Trainee>();
            MainSkills = new List<MainSkill>();
            Mentors = new List<Mentor>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public virtual ICollection<Trainee> Trainees { get; set; }
        public virtual ICollection<MainSkill> MainSkills { get; set; }
        public virtual ICollection<Mentor> Mentors { get; set; }
    }
}
