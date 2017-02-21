using System.Collections.Generic;
using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models
{
    [JsonObject]
    [JsonNetModel]
    public class MentorReviewJsonModel
    {
        [JsonProperty("TraineeId")]
        public int ReviewedTraineeId { get; set; }
        public List<GradeViewModel> SkillGrades { get; set; }
        public int MentorId { get; set; }
        public string WorkComment { get; set; }
    }
}