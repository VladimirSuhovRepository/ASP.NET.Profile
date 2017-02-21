using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Profile.JiraRestClient.Entities.Issues;
using Profile.JiraRestClient.Utils;

namespace Profile.JiraRestClient.Entities.Searching
{
    public class SearchResponse
    {
        /// <summary>
        /// A comma-separated list of the parameters to expand.
        /// </summary>
        [JsonProperty("expand")]
        public string Expand { get; set; }

        /// <summary>
        /// The index of the first issue to return (0-based).
        /// </summary>
        [JsonProperty("startAt")]
        public int StartAt { get; set; }

        /// <summary>
        /// The maximum number of issues to return (defaults to 50).
        /// The maximum allowable value is dictated by the JIRA property 'jira.search.views.default.max'.
        /// If you specify a value that is higher than this number, your search results will be truncated.
        /// If you specify -1 then all search results will be queried.
        /// </summary>
        [JsonProperty("maxResults")]
        public int MaxResults { get; set; }

        /// <summary>
        /// Total number of entities contained in all pages.
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>
        /// Issues records appropriates to the search request.
        /// </summary>
        [JsonProperty("issues")]
        public List<Issue> Issues { get; set; }

        public string GetTotalEstimatedTime()
        {
            var totalEstimatedTime = Issues.Sum(p => p.Fields.TimeTracking.OriginalEstimateSeconds);
            return JiraTimeConverter.ToJiraTimeFormat(totalEstimatedTime);
        }

        public string GetTotalLoggedTime()
        {
            var totalLoggedTime = Issues.Sum(p => p.Fields.TimeTracking.TimeSpentSeconds);
            return JiraTimeConverter.ToJiraTimeFormat(totalLoggedTime);
        }
    }
}
