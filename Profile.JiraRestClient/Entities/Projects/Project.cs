using Newtonsoft.Json;
using Profile.JiraRestClient.Entities.Misc;

namespace Profile.JiraRestClient.Entities.Projects
{
    /// <summary>
    /// A class representing a JIRA project description
    /// </summary>
    public class Project
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("avatarUrls")]
        public AvatarUrls AvatarUrls { get; set; }

        [JsonProperty("projectCategory")]
        public ProjectCategory ProjectCategory { get; set; }

        [JsonProperty("projectTypeKey")]
        public string ProjectTypeKey { get; set; }
    }
}
