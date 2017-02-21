using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.DAL.Entities
{
    public class Avatar
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string Base64Url { get; set; }
        
        public virtual User User { get; set; }
    }
}
