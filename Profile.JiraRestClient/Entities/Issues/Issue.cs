using Newtonsoft.Json;

namespace Profile.JiraRestClient.Entities.Issues
{
    /// <summary>
    /// A class representing a JIRA issue description
    /// </summary>
    public class Issue
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("fields")]
        public Fields Fields { get; set; }
    }
}
