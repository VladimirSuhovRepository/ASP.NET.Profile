namespace Profile.DAL.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int? Mark { get; set; }

        public int? ReviewId { get; set; }
        public virtual Review Review { get; set; }

        public int? SkillId { get; set; }
        public virtual Skill Skill { get; set; }
    }
}
