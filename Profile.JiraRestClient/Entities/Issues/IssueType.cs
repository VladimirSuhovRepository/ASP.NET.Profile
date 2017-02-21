using Newtonsoft.Json;

namespace Profile.JiraRestClient.Entities.Issues
{
    public class IssueType
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("subtask")]
        public bool IsSubtask { get; set; }

        [JsonProperty("avatarId")]
        public string AvatarId { get; set; }
    }
}
