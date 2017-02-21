using System;
using System.Collections.Generic;

namespace Profile.DAL.Entities
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Size { get; set; }

        public int? TraineeProfileId { get; set; }
        public virtual TraineeProfile TraineeProfile { get; set; }

        public virtual FileData FileData { get; set; }
    }
}
