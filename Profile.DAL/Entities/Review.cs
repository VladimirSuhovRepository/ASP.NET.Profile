using System.Collections.Generic;

namespace Profile.DAL.Entities
{
    public class Review
    {
        public Review()
        {
            Grades = new List<Grade>();
        }

        public int Id { get; set; }
        public ReviewType ReviewType { get; set; }

        public int ReviewerId { get; set; }
        public virtual User Reviewer { get; set; }

        public int ReviewedTraineeId { get; set; }
        public virtual Trainee ReviewedTrainee { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
    }
}
