using System.Collections.Generic;

namespace Profile.UI.Models
{
    public class MentorReviewViewModel
    {
        public MentorReviewViewModel()
        {
            MainSkillGrades = new List<GradeViewModel>();
            SoftSkillGrades = new List<GradeViewModel>();
        }

        public int Id { get; set; }
        public TraineeViewModel ReviewedTrainee { get; set; }
        public List<GradeViewModel> SoftSkillGrades { get; set; }
        public List<GradeViewModel> MainSkillGrades { get; set; }
        public int MentorId { get; set; }
    }
}