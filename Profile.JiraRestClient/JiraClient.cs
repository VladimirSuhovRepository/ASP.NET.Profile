using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using Profile.JiraRestClient.Entities.Searching;
using Profile.JiraRestClient.Exceptions;
using Profile.JiraRestClient.Interfaces;
using RestSharp;
using RestSharp.Deserializers;

namespace Profile.JiraRestClient
{
    public class JiraClient : IJiraClient
    {
        private readonly string _jiraUrl;
        private readonly string _jiraUserName;
        private readonly string _jiraPassword;
        private readonly JsonDeserializer _deserializer;

        public JiraClient(string jiraUrl, string jiraUserName, string jiraPassword)
        {
            _jiraUrl = new Uri(new Uri(jiraUrl), "rest/api/2/").ToString();
            _jiraUserName = jiraUserName;
            _jiraPassword = jiraPassword;

            _deserializer = new JsonDeserializer();
        }

        public SearchResponse SearchIssuesByQuery(JqlQuery jqlQuery, List<string> fields, int startAt, int maxResults)
        {
            var searchRequest = new SearchRequest
            {
                JqlQuery = jqlQuery.CreateJqlQuery(),
                Fields = fields,
                StartAt = startAt,
                MaxResults = maxResults
            };

            try
            {
                var request = CreateRequest(Method.GET, searchRequest.CreateSearchRequest());
                var response = ExecuteRequest(request);

                AssertStatus(response, HttpStatusCode.OK);

                var data = _deserializer.Deserialize<SearchResponse>(response);
                return data;
            }
            catch (Exception ex)
            {
                Trace.TraceError("SearchIssuesByQuery(jqlQuery, fields, startAt, maxResults) error: {0}", ex);
                throw new JiraClientException("Could not load issues", ex);
            }
        }

        private IRestRequest CreateRequest(Method method, string path)
        {
            var request = new RestRequest { Method = method, Resource = path, RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_jiraUserName}:{_jiraPassword}")));

            return request;
        }

        private IRestResponse ExecuteRequest(IRestRequest request)
        {
            var client = new RestClient(_jiraUrl);
            return client.Execute(request);
        }

        private void AssertStatus(IRestResponse response, HttpStatusCode status)
        {
            if (response.ErrorException != null)
            {
                throw new JiraClientException("Transport level error: " + response.ErrorMessage, response.ErrorException);
            }

            if (response.StatusCode != status)
            {
                throw new JiraClientException("JIRA returned wrong status: " + response.StatusDescription, response.Content);
            }
        }
    }
}
