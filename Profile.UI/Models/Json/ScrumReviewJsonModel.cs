using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class ScrumReviewJsonModel
    {
        [JsonProperty("id")]
        public int ReviewedTraineeId { get; set; }

        [JsonProperty("scrumId")]
        public int ScrumMasterId { get; set; }

        [JsonProperty("review")]
        public string Comment { get; set; }

        [JsonProperty("mark")]
        public int Mark { get; set; }
    }
}