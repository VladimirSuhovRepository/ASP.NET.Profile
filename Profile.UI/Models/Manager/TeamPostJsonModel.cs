using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Manager
{
    [JsonObject]
    [JsonNetModel]
    public class TeamPostJsonModel
    {
        public TeamPostJsonModel()
        {
            Trainees = new List<ManagerTraineeJsonModel>();
        }

        [JsonProperty("teamName")]
        public string Number { get; set; }

        [JsonProperty("teamStartDate")]
        [UnixTimeBind]
        public DateTime StartDate { get; set; }

        [JsonProperty("teamFinishDate")]
        [UnixTimeBind]
        public DateTime FinishDate { get; set; }

        [JsonProperty("teamId")]
        public int Id { get; set; }

        [JsonProperty("scrumId")]
        public int? ScrumId { get; set; }

        public List<ManagerTraineeJsonModel> Trainees { get; set; }
    }
}