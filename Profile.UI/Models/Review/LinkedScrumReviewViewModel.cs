namespace Profile.UI.Models.Review
{
    public class LinkedScrumReviewViewModel : ScrumReviewViewModel, ILinkedReviewViewModel
    {
        private const string ScrumReviewPartialViewName = "_ScrumReviewPartial";

        public string PartialViewName => ScrumReviewPartialViewName;
    }
}