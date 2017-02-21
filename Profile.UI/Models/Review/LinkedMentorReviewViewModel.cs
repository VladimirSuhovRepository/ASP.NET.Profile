namespace Profile.UI.Models.Review
{
    public class LinkedMentorReviewViewModel : ProfileMentorReviewViewModel, ILinkedReviewViewModel
    {
        private const string MentorReviewPartialViewName = "_PartialProfileMentorReview";

        public string PartialViewName => MentorReviewPartialViewName;
    }
}