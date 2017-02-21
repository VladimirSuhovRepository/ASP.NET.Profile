using Newtonsoft.Json;

namespace Profile.JiraRestClient.Entities.Issues
{
    public class TimeTracking
    {
        [JsonProperty("originalEstimate")]
        public string OriginalEstimate { get; set; }

        [JsonProperty("originalEstimateSeconds")]
        public int OriginalEstimateSeconds { get; set; }

        [JsonProperty("timeSpent")]
        public string TimeSpent { get; set; }

        [JsonProperty("timeSpentSeconds")]
        public int TimeSpentSeconds { get; set; }

        [JsonProperty("remainingEstimate")]
        public string RemainingEstimate { get; set; }

        [JsonProperty("remainingEstimateSeconds")]
        public int RemainingEstimateSeconds { get; set; }
    }
}
