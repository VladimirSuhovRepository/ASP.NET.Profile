using System;
using System.Collections.Generic;

namespace Profile.DAL.Entities
{
    public class Language
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Level { get; set; }

        public int? TraineeProfileId { get; set; }
        public virtual TraineeProfile TraineeProfile { get; set; }
    }
}
