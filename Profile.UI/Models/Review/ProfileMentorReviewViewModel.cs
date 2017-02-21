using System.Collections.Generic;

namespace Profile.UI.Models.Review
{
    public class ProfileMentorReviewViewModel
    {
        public ProfileMentorReviewViewModel()
        {
            MainSkillGrades = new List<GradeViewModel>();
            SoftSkillGrades = new List<GradeViewModel>();
        }

        public int Id { get; set; }
        public List<GradeViewModel> SoftSkillGrades { get; set; }
        public List<GradeViewModel> MainSkillGrades { get; set; }
        public int MentorId { get; set; }
        public string MentorFullname { get; set; }
        public string MentorSpecializationName { get; set; }
        public string WorkComment { get; set; }
    }
}