using Newtonsoft.Json;

namespace Profile.JiraRestClient.Entities.Projects
{
    public class ProjectCategory
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
