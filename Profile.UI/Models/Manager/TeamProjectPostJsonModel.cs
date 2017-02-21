using System;
using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Manager
{
    [JsonObject]
    [JsonNetModel]
    public class TeamProjectPostJsonModel
    {
        [JsonProperty("projectId")]
        public int ProjectId { get; set; }
        [JsonProperty("projectName")]
        public string Name { get; set; }
        [JsonProperty("projectStartDate")]
        [UnixTimeBind]
        public DateTime StartDate { get; set; }
        [JsonProperty("projectFinishDate")]
        [UnixTimeBind]
        public DateTime FinishDate { get; set; }
        public TeamPostJsonModel Team { get; set; }
    }
}