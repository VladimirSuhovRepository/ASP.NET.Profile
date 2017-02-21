using System.Collections.Generic;
using System.Linq;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class TraineeReviewProvider : ITraineeReviewProvider
    {
        private readonly IProfileContext _context;

        public TraineeReviewProvider(IProfileContext context)
        {
            _context = context;
        }

        public List<Grade> GetTeamGradesOnTrainee(int traineeId)
        {
            return _context.Reviews
                .Where(r => r.ReviewedTraineeId == traineeId &&
                    r.ReviewType == ReviewType.TraineeReview)
                .SelectMany(r => r.Grades)
                .ToList();
        }

        public void AddTraineeReview(Review review)
        {
            review.ReviewType = ReviewType.TraineeReview;
            _context.Reviews.Add(review);

            _context.SaveChanges();
        }

        public List<Skill> GetAbilities()
        {
            return _context.Skills
                .Where(s => s.SkillType == SkillType.Ability ||
                    s.SkillType == SkillType.Strenghts ||
                    s.SkillType == SkillType.Weaknesses)
                .ToList();
        }

        public Review CreateEmptyForTrainee(int traineeId, int reviewerId)
        {
            var trainee = _context.Trainees.Find(traineeId);

            var review = new Review
            {
                ReviewType = ReviewType.TraineeReview,
                ReviewedTrainee = trainee,
                ReviewedTraineeId = trainee.Id,
                ReviewerId = reviewerId
            };

            var skills = _context.Skills.Where(s => s.SkillType == SkillType.Ability);

            foreach (var skill in skills)
            {
                review.Grades.Add(new Grade
                {
                    Review = review,
                    Skill = skill,
                    SkillId = skill.Id
                });
            }

            return review;
        }
    }
}
