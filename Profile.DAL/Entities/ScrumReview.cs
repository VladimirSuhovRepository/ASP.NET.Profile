using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.DAL.Entities
{
    [Table("ScrumReviews")]
    public class ScrumReview : Review
    {
        public int Grade { get; set; }
        public string Comment { get; set; }

        public int? ScrumMasterId { get; set; }
        public virtual ScrumMaster ScrumMaster { get; set; }
    }
}
