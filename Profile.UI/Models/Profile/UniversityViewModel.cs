using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Profile
{
    [JsonObject]
    [JsonNetModel]
    public class UniversityViewModel
    {
        [JsonProperty("universityId")]
        public int Id { get; set; }

        [JsonProperty("universityEstablishment")]
        public string EducationalInstitution { get; set; }

        [JsonProperty("timePeriod")]
        public string TrainingPeriod { get; set; }

        [JsonProperty("specialization")]
        public string Specialization { get; set; }

        [JsonProperty("specialty")]
        public string Specialty { get; set; }

        [JsonProperty("additionalInformation")]
        [DataType(DataType.MultilineText)]
        public string AdditionalInformation { get; set; }
    }
}