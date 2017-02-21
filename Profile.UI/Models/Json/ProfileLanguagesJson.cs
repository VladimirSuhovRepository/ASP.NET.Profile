using System.Collections.Generic;
using Newtonsoft.Json;
using Profile.UI.Attributes;
using Profile.UI.Extentions;
using Profile.UI.Models.Profile;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class ProfileLanguagesJson
    {
        public ProfileLanguagesJson()
        {
            Languages = new List<LanguageViewModel>();
        }

        [JsonProperty("profileId")]
        public int ProfileId { get; set; }

        [JsonProperty("languages")]
        public List<LanguageViewModel> Languages { get; set; }

        public void TrimAndUppercaseFirst()
        {
            Languages.ForEach(language => language.Name = language.Name.TrimAndUppercaseFirst());
        }
    }
}
