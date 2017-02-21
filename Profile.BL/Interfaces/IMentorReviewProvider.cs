using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface IMentorReviewProvider
    {
        Review CreateEmptyForTrainee(int traineeId);
        Review CreateEmptyForTrainee(Trainee trainee);
        void AddMentorReview(Review review);
        Review GetMentorReviewForTrainee(int traineeId);
    }
}
