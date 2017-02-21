using Newtonsoft.Json;
using Profile.JiraRestClient.Entities.Projects;

namespace Profile.JiraRestClient.Entities.Issues
{
    public class Fields
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("timetracking")]
        public TimeTracking TimeTracking { get; set; }

        [JsonProperty("issuetype")]
        public IssueType IssueType { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("assignee")]
        public JiraUser Assignee { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }
    }
}
