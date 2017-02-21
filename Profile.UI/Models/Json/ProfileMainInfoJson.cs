using Newtonsoft.Json;
using Profile.UI.Attributes;
using Profile.UI.Extentions;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class ProfileMainInfoJson
    {
        [JsonProperty("profileId")]
        public int Id { get; set; }

        [JsonProperty("Phone")]
        public string Phone { get; set; }

        [JsonProperty("Skype")]
        public string Skype { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Linkedin")]
        public string LinkedIn { get; set; }

        [JsonProperty("DesirablePosition")]
        public string DesirablePosition { get; set; }

        [JsonProperty("ExperienceAtITLab")]
        public string ExperienceAtITLab { get; set; }

        [JsonProperty("ProfessionalPurposes")]
        public string ProfessionalPurposes { get; set; }

        [JsonProperty("CurrentPosition")]
        public string CurrentPosition { get; set; }

        [JsonProperty("CurrentWork")]
        public string CurrentWork { get; set; }

        [JsonProperty("EmploymentDuration")]
        public string EmploymentDuration { get; set; }

        [JsonProperty("JobDuties")]
        public string JobDuties { get; set; }

        public void TrimAndUppercaseFirst()
        {
            Email = Email?.TrimAndUppercaseFirst();
            Skype = Skype?.TrimAndUppercaseFirst();
            LinkedIn = LinkedIn?.TrimAndUppercaseFirst();

            DesirablePosition = DesirablePosition.TrimAndUppercaseFirst();
            ExperienceAtITLab = ExperienceAtITLab.TrimAndUppercaseFirst();
            ProfessionalPurposes = ProfessionalPurposes.TrimAndUppercaseFirst();

            CurrentPosition = CurrentPosition.TrimAndUppercaseFirst();
            CurrentWork = CurrentWork.TrimAndUppercaseFirst();
            EmploymentDuration = EmploymentDuration.TrimAndUppercaseFirst();
        }      
    }
}
