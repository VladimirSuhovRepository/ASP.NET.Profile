using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using Profile.DAL.Identity.Entities;

namespace Profile.DAL.Entities
{
    public class Role : IdentityRole<int, UserRole>
    {
        public Role()
        {
            Permissions = new List<Permission>();
        }

        public Role(RoleType type)
            : this()
        {
            Type = type;
            Name = Type.ToString();
        }

        public RoleType Type { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
