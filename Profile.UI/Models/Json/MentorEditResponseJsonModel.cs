using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class MentorEditResponseJsonModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Skype { get; set; }
        public string LinkedIn { get; set; }
        public string AvatarUrl { get; set; }
    }
}