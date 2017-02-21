using System.Linq;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class ScrumReviewProvider : IScrumReviewProvider
    {
        private IProfileContext _context;

        public ScrumReviewProvider(IProfileContext profileContext)
        {
            _context = profileContext;
        }

        public Review GetReviewForTrainee(int traineeId)
        {
            return _context.Reviews.FirstOrDefault(
                sr => sr.ReviewedTraineeId == traineeId &&
                sr.ReviewType == ReviewType.ScrumReview);
        }

        public Review CreateEmptyForTrainee(int traineeId)
        {
            var trainee = _context.Trainees.Find(traineeId);

            return new Review
            {
                ReviewType = ReviewType.ScrumReview,
                ReviewedTrainee = trainee,
                ReviewedTraineeId = trainee.Id
            };
        }

        public void AddScrumReview(Review review)
        {
            review.ReviewType = ReviewType.ScrumReview;
            _context.Reviews.Add(review);

            _context.SaveChanges();
        }
    }
}
