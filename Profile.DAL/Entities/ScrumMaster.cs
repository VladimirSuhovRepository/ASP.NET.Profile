using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.DAL.Entities
{
    public class ScrumMaster
    {
        public ScrumMaster()
        {
            Groups = new List<Group>();
        }

        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public int? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group CurrentGroup { get; set; }

        public bool HasProject
        {
            get
            {
                if (this.CurrentGroup == null) return false;

                return this.CurrentGroup.Project.FinishDate > System.DateTime.Now;
            }
        }

        public static ScrumMaster GetBlank(User user)
        {
            return new ScrumMaster
            {
                User = user
            };
        }
    }
}
