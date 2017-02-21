using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.DAL.Entities
{
    public class Trainee
    {
        public Trainee()
        {
            Issues = new List<Issue>();
            ReviewsOnMe = new List<Review>();
        }

        public bool IsAllowed { get; set; }

        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }
        public virtual User User { get; set; }

        public virtual TraineeProfile TraineeProfile { get; set; }

        public virtual ICollection<Issue> Issues { get; set; }

        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }

        public int SpecializationId { get; set; }
        public virtual Specialization Specialization { get; set; }

        public int? MentorId { get; set; }
        public virtual Mentor Mentor { get; set; }

        public virtual ICollection<Review> ReviewsOnMe { get; set; }

        public static Trainee GetBlank(User user, Specialization specialization)
        {
            return new Trainee
            {
                User = user,
                Specialization = specialization,
                TraineeProfile = new TraineeProfile
                {
                    TraineeId = user.Id,
                }
            };
        }
    }
}
