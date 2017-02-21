using System.Collections.Generic;
using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface ITraineeReviewProvider
    {
        List<Grade> GetTeamGradesOnTrainee(int traineeId);
        List<Skill> GetAbilities();
        Review CreateEmptyForTrainee(int traineeId, int reviewerId);
        void AddTraineeReview(Review review);
    }
}