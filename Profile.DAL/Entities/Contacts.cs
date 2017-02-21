using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.DAL.Entities
{
    public class Contacts
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Skype { get; set; }
        public string LinkedIn { get; set; }
        public string Company { get; set; }

        public virtual User User { get; set; }
    }
}
