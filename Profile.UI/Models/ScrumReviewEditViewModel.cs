namespace Profile.UI.Models
{
    public class ScrumReviewEditViewModel
    {
        public TraineeViewModel ReviewedTrainee { get; set; }
        public string Comment { get; set; }
        public int? Mark { get; set; }
        public bool IsReviewed { get; set; }
    }
}