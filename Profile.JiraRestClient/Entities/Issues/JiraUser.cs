using Newtonsoft.Json;
using Profile.JiraRestClient.Entities.Misc;

namespace Profile.JiraRestClient.Entities.Issues
{
    public class JiraUser
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("emailAddress")]
        public string Email { get; set; }

        [JsonProperty("avatarUrls")]
        public AvatarUrls AvatarUrls { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("active")]
        public bool IsActive { get; set; }
    }
}
