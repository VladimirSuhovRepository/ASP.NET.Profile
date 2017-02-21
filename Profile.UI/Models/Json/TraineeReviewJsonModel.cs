using System.Collections.Generic;
using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class TraineeReviewJsonModel
    {
        [JsonProperty("TraineeId")]
        public int ReviewedTraineeId { get; set; }
        [JsonProperty("SkillGrades")]
        public List<GradeViewModel> Grades { get; set; }
        public int ReviewerId { get; set; }
    }
}