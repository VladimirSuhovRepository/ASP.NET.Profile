using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Profile
{
    [JsonObject]
    [JsonNetModel]
    public class LanguageViewModel
    {
        [JsonProperty("languageId")]
        public int Id { get; set; }

        [JsonProperty("languageName")]
        public string Name { get; set; }

        [JsonProperty("languageLevel")]
        public string Level { get; set; }
    }
}