using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.DAL.Entities
{
    public class Link
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public int? TraineeProfileId { get; set; }
        public virtual TraineeProfile TraineeProfile { get; set; }
    }
}
