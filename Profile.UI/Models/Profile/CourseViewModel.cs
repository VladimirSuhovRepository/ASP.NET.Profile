using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Profile
{
    [JsonObject]
    [JsonNetModel]
    public class CourseViewModel
    {
        [JsonProperty("courseId")]
        public int Id { get; set; }

        [JsonProperty("courseEstablishment")]
        public string EducationalEstablishment { get; set; }

        [JsonProperty("courseName")]
        public string CourseName { get; set; }

        [JsonProperty("timePeriod")]
        public string TimePeriod { get; set; }

        [JsonProperty("additionalInformation")]
        public string AdditionalInformation { get; set; }
    }
}