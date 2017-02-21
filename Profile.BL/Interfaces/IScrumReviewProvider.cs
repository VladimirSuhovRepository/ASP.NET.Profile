using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface IScrumReviewProvider
    {
        Review GetReviewForTrainee(int traineeId);
        Review CreateEmptyForTrainee(int traineeId);
        void AddScrumReview(Review review);
    }
}
