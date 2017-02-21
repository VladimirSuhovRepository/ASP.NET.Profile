using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class LinkJsonRequestModel
    {
        [JsonProperty("LinkId")]
        public int Id { get; set; }

        [JsonProperty("Url")]
        public string Url { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("profileId")]
        public int TraineeProfileId { get; set; }
    }
}