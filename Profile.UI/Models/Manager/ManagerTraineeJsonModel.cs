using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Manager
{
    [JsonObject]
    [JsonNetModel]
    public class ManagerTraineeJsonModel
    {
        [JsonProperty("traineeId")]
        public int Id { get; set; }

        [JsonProperty("mentorId")]
        public int MentorId { get; set; }
        public string FullName { get; set; }
        public string Specialization { get; set; }
        public string MentorName { get; set; }
    }
}