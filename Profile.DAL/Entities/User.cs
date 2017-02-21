using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Profile.DAL.Identity.Entities;

namespace Profile.DAL.Entities
{
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        [NotMapped]
        public string Login
        {
            get { return UserName; }

            set { UserName = value; }
        }

        public string FullName { get; set; }
        public string ResetPasswordToken { get; set; }
        public bool IsInArchive { get; set; }

        public virtual Contacts Contacts { get; set; }

        public virtual Avatar Avatar { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<User, int> manager)
        {
            // Note the authenticationType must match the one defined in
            // CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = await manager.CreateIdentityAsync(
                this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }
    }
}
