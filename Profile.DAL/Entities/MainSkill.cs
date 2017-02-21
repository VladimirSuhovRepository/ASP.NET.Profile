using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.DAL.Entities
{
    [Table("MainSkills")]
    public class MainSkill : Skill
    {
        public MainSkill()
        {
            SkillType = SkillType.MainSkill;
            TraineeProfiles = new List<TraineeProfile>();
        }

        public virtual ICollection<TraineeProfile> TraineeProfiles { get; set; }

        public int? SpecializationId { get; set; }
        public virtual Specialization Specialization { get; set; }
    }
}
