using System.Collections.Generic;
using Newtonsoft.Json;

namespace Profile.JiraRestClient.Entities.Searching
{
    public class SearchRequest
    {
        public SearchRequest()
        {
            Fields = new List<string>();
        }

        /// <summary>
        /// A JQL query string.
        /// </summary>
        [JsonProperty("jql")]
        public string JqlQuery { get; set; }

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
        /// The list of fields to return for each issue. By default, all navigable fields are returned.
        /// </summary>
        [JsonProperty("fields")]
        public List<string> Fields { get; set; }

        /// <summary>
        /// Whether to validate the JQL query (default is true).
        /// </summary>
        [JsonProperty("validateQuery")]
        public bool IsValidateQuery { get; set; }

        /// <summary>
        /// A comma-separated list of the parameters to expand.
        /// </summary>
        [JsonProperty("expand")]
        public string Expand { get; set; }

        public string CreateSearchRequest()
        {
            var request = $"search?jql={JqlQuery}&startAt={StartAt}&maxResults={MaxResults}";

            if (Fields != null) request += $"&fields={string.Join(",", Fields)}";

            return request;
        }
    }
}
