using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface IProfileProvider
    {
        TraineeProfile GetProfile(int profileId);
        TraineeProfile GetProfileByTraineeId(int traineeId);
        TraineeProfile UpdateMainInfo(TraineeProfile profile);
        TraineeProfile UpdateQualification(TraineeProfile profile);

        // TODO: Will be moved to trainee entity such as properties
        bool IsTraineeHavingMentorReview(int traineeId);
        bool IsTraineeHavingScrumReview(int traineeId);
        bool IsTraineeHavingTeamReview(int traineeId);
        double GetTraineeRating(int traineeId);
    }
}
