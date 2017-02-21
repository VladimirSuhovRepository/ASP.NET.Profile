using System.Collections.Generic;

namespace Profile.DAL.Entities
{
    public class Permission
    {
        public Permission()
        {
            Roles = new List<Role>();
        }

        public Permission(PermissionType type)
            : this()
        {
            Type = type;
        }

        public int Id { get; set; }
        public PermissionType Type { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
