using Profile.UI.Models.Profile;

namespace Profile.UI.Models.Review
{
    public class ProfileReviewViewModel : ProfileMainInfoViewModel
    {
        public bool IsTraineeHavingMentorReview { get; set; }
        public bool IsTraineeHavingScrumReview { get; set; }
        public bool IsTraineeHavingTeamReview { get; set; }
        public ILinkedReviewViewModel RenderingReview { get; set; }
    }
}