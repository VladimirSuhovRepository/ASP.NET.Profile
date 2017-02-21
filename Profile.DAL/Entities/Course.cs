using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.DAL.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string EducationalEstablishment { get; set; }
        public string CourseName { get; set; }
        public string TimePeriod { get; set; }
        public string AdditionalInformation { get; set; }

        public int? TraineeProfileId { get; set; }
        public virtual TraineeProfile TraineeProfile { get; set; }
    }
}
