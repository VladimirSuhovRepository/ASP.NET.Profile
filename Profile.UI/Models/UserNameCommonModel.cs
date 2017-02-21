using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models
{
    [JsonObject]
    [JsonNetModel]
    public class UserNameCommonModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string FullName { get; set; }
    }
}