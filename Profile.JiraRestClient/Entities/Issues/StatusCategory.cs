using Newtonsoft.Json;

namespace Profile.JiraRestClient.Entities.Issues
{
    public class StatusCategory
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("colorName")]
        public string ColorName { get; set; }
    }
}
