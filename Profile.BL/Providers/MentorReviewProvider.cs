using System.Collections.Generic;
using System.Linq;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class MentorReviewProvider : IMentorReviewProvider
    {
        private IProfileContext _context;

        public MentorReviewProvider(IProfileContext profileContext)
        {
            _context = profileContext;
        }

        public Review CreateEmptyForTrainee(Trainee trainee)
        {
            if (trainee == null) return null;

            Review review = new Review
            {
                ReviewType = ReviewType.MentorReview,
                Reviewer = trainee.Mentor.User,
                ReviewerId = trainee.MentorId.Value,
                ReviewedTrainee = trainee,
                ReviewedTraineeId = trainee.Id
            };

            var grades = new List<Grade>();
            var mainSkills = trainee.Specialization.MainSkills;

            foreach (var skill in mainSkills)
            {
                grades.Add(new Grade
                {
                    Review = review,
                    Skill = skill,
                    SkillId = skill.Id
                });
            }

            var softSkills = trainee.TraineeProfile.SoftSkills;

            foreach (var skill in softSkills)
            {
                grades.Add(new Grade
                {
                    Review = review,
                    Skill = skill,
                    SkillId = skill.Id
                });
            }

            review.Grades = grades;
            return review;
        }

        public Review CreateEmptyForTrainee(int traineeId)
        {
            var trainee = _context.Trainees.Find(traineeId);

            return CreateEmptyForTrainee(trainee);
        }

        public void AddMentorReview(Review review)
        {
            review.ReviewType = ReviewType.MentorReview;
            _context.Reviews.Add(review);

            _context.SaveChanges();
        }

        public Review GetMentorReviewForTrainee(int traineeId)
        {
            return _context.Reviews.SingleOrDefault(
                r => r.ReviewedTraineeId == traineeId &&
                r.ReviewType == ReviewType.MentorReview);
        }
    }
}
