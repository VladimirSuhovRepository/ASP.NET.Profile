using System.Collections.Generic;
using Profile.JiraRestClient.Entities.Searching;

namespace Profile.JiraRestClient.Interfaces
{
    public interface IJiraClient
    {
        SearchResponse SearchIssuesByQuery(JqlQuery jqlQuery, List<string> fields, int startAt, int maxResults);
    }
}
