using System;
using System.Collections.Generic;

namespace Profile.DAL.Entities
{
    public class University
    {
        public int Id { get; set; }
        public string EducationalInstitution { get; set; }
        public string TrainingPeriod { get; set; }
        public string Specialization { get; set; }
        public string Specialty { get; set; }
        public string AdditionalInformation { get; set; }

        public int? TraineeProfileId { get; set; }
        public virtual TraineeProfile TraineeProfile { get; set; }
    }
}
