using Profile.DAL.Entities;

namespace Profile.UI.Models
{
    public class SkillViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SkillType SkillType { get; set; }
    }
}