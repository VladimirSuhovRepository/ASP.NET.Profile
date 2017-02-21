namespace Profile.UI.Models
{
    public class GradeViewModel
    {
        public int Id { get; set; }
        public SkillViewModel Skill { get; set; }
        public string Comment { get; set; }
        public int? Mark { get; set; }
        public int? SkillId { get; set; }
    }
}