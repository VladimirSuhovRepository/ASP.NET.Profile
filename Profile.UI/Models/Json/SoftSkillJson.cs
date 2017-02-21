using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class SoftSkillJson
    {
        [JsonProperty("idSoftSkill")]
        public int Id { get; set; }

        [JsonProperty("softSkillName")]
        public string Name { get; set; }
    }
}