using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class LinkJsonResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("link")]
        public string Url { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}