using System.Collections.Generic;
using System.Linq;

namespace Profile.UI.Models.Review
{
    public class TeamReviewedSkillViewModel
    {
        public TeamReviewedSkillViewModel()
        {
            SkillReviews = new List<TeammateSkillGradeViewModel>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool HasMark => SkillReviews.Any(r => r.Mark.HasValue);

        public List<TeammateSkillGradeViewModel> SkillReviews { get; set; }
    }
}