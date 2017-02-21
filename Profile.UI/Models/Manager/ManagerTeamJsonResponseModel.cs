using System.Collections.Generic;
using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Manager
{
    [JsonObject]
    [JsonNetModel]
    public class ManagerTeamJsonResponseModel
    {
        public ManagerTeamJsonResponseModel()
        {
            Trainees = new List<ManagerTraineeJsonModel>();
        }

        public string ScrumName { get; set; }
        public string ProjectName { get; set; }
        public string TeamName { get; set; }

        [JsonProperty("Trainee")]
        public List<ManagerTraineeJsonModel> Trainees { get; set; }
    }
}