using System.Collections.Generic;
using Newtonsoft.Json;
using Profile.UI.Attributes;
using Profile.UI.Extentions;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class SkillsContainerJson
    {
        public SkillsContainerJson()
        {
            SoftSkills = new List<SoftSkillJson>();
            Specializations = new List<SpecializationJson>();
        }

        [JsonProperty("profileId")]
        public int ProfileId { get; set; }

        [JsonProperty("MainSkills")]
        public List<SpecializationJson> Specializations { get; set; }

        [JsonProperty("SoftSkills")]
        public List<SoftSkillJson> SoftSkills { get; set; }

        public void TrimAndUppercaseFirst()
        {
            SoftSkills.ForEach(skill => skill.Name = skill.Name.TrimAndUppercaseFirst());
        }
    }
}