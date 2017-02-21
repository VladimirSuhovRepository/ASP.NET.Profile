using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.DAL.Entities
{
    [Table("SoftSkills")]
    public class SoftSkill : Skill
    {
        public SoftSkill()
        {
            SkillType = SkillType.SoftSkill;
        }

        public int? TraineeProfileId { get; set; }
        public virtual TraineeProfile TraineeProfile { get; set; }
    }
}
