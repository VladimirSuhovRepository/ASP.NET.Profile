namespace Profile.UI.Models.Profile
{
    public class ProfileMainInfoViewModel
    {
        public int TraineeId { get; set; }
        public string TraineeFullName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; }
        public int GpoupId { get; set; }
        public string GroupNumber { get; set; }
        public double Rating { get; set; }
        public bool HasReviews { get; set; }
    }
}