namespace Profile.UI.Models.Review
{
    public class LinkedTeamReviewViewModel : ProfileTeamReviewViewModel, ILinkedReviewViewModel
    {
        private const string TeamReviewPartialViewName = "_PartialProfileTeamReview";

        public string PartialViewName => TeamReviewPartialViewName;
    }
}