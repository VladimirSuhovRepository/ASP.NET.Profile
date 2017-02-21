using System.Web;

namespace Profile.UI.Models.Json
{
    public class MentorEditRequestJsonModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Skype { get; set; }
        public string LinkedIn { get; set; }
        public bool IsAvatarDeleting { get; set; }
        public HttpPostedFileBase NewAvatar { get; set; }
    }
}