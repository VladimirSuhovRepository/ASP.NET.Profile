using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.DAL.Entities
{
    public class Mentor
    {
        public Mentor()
        {
            Trainees = new List<Trainee>();
        }

        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }
        public virtual User User { get; set; }

        public int SpecializationId { get; set; }
        public virtual Specialization Specialization { get; set; }

        public virtual ICollection<Trainee> Trainees { get; set; }

        public static Mentor GetBlank(User user, Specialization specialization)
        {
            return new Mentor
            {
                User = user,
                Specialization = specialization
            };
        }
    }
}
