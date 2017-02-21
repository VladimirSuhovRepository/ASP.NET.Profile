using System.Collections.Generic;

namespace Profile.UI.Models
{
    public class TraineeReviewViewModel
    {
        public TraineeReviewViewModel()
        {
            Skills = new List<SkillViewModel>();

            Strengths = new SkillViewModel();
            Weakness = new SkillViewModel();
        }

        public int Id { get; set; }
        public TraineeViewModel ReviewedTrainee { get; set; }
        public List<SkillViewModel> Skills { get; set; }
        public int ReviewerId { get; set; }

        public SkillViewModel Strengths { get; set; }
        public SkillViewModel Weakness { get; set; }
    }
}