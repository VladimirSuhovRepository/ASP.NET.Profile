using System;
using System.Collections.Generic;
using System.Globalization;

namespace Profile.JiraRestClient.Entities.Searching
{
    /// <summary>
    /// A class representing a JQL query that sending to the JIRA server
    /// </summary>
    public class JqlQuery
    {
        public string Assignee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Project { get; set; }
        public string Status { get; set; }
        public List<string> IssueTypes { get; set; }

        public string CreateJqlQuery()
        {
            var queryParts = new List<string>();

            if (!string.IsNullOrEmpty(Assignee)) queryParts.Add($"assignee={Assignee}");

            if (!string.IsNullOrEmpty(Project)) queryParts.Add($"project={Project}");

            if (StartDate != default(DateTime))
            {
                queryParts.Add($"updated>=\"{ToJqlDate(StartDate)}\"");
            }

            if (ReleaseDate != default(DateTime))
            {
                queryParts.Add($"updated<=\"{ToJqlDate(ReleaseDate)}\"");
            }

            if (!string.IsNullOrEmpty(Status)) queryParts.Add($"status={Status}");

            if (IssueTypes.Count > 0)
            {
                queryParts.Add($"issueType in ({string.Join(",", IssueTypes)})");
            }

            return string.Join(" AND ", queryParts);
        }

        private string ToJqlDate(DateTime date)
        {
            return date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
        }
    }
}
