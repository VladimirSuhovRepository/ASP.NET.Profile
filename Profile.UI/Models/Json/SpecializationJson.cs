using System.Collections.Generic;
using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class SpecializationJson
    {
        public SpecializationJson()
        {
            IdSkills = new List<int>();
        }

        [JsonProperty("SpecId")]
        public int Id { get; set; }

        [JsonProperty("SkillsId")]
        public List<int> IdSkills { get; set; }
    }
}