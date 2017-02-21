using Newtonsoft.Json;
using Profile.UI.Attributes;
using Profile.UI.Extentions;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class ProfileQualificationJson
    {
        [JsonProperty("profileId")]
        public int Id { get; set; }

        [JsonProperty("Strengths")]
        public string Strengths { get; set; }

        [JsonProperty("Weaknesses")]
        public string Weaknesses { get; set; }

        public void TrimAndUppercaseFirst()
        {
            Strengths = Strengths.TrimAndUppercaseFirst();
            Weaknesses = Weaknesses.TrimAndUppercaseFirst();
        }
    }
}
