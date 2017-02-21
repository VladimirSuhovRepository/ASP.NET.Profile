using System.Collections.Generic;

namespace Profile.UI.Models.Review
{
    public class ProfileTeamReviewViewModel
    {
        public ProfileTeamReviewViewModel()
        {
            Skills = new List<TeamReviewedSkillViewModel>();
        }

        public List<TeamReviewedSkillViewModel> Skills { get; set; }
    }
}