using Newtonsoft.Json;

namespace Profile.JiraRestClient.Entities.Misc
{
    public class AvatarUrls
    {
        [JsonProperty("16x16")]
        public string Size16 { get; set; }

        [JsonProperty("24x24")]
        public string Size24 { get; set; }

        [JsonProperty("32x32")]
        public string Size32 { get; set; }

        [JsonProperty("48x48")]
        public string Size48 { get; set; }
    }
}
