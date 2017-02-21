using System.Collections.Generic;
using System.Linq;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;
using Profile.JiraRestClient.Entities.Searching;
using Profile.JiraRestClient.Interfaces;

namespace Profile.BL.Providers
{
    public class JiraProvider : IJiraProvider
    {
        private IJiraClient _client;
        private IProfileContext _context;
        private ITraineeProvider _traineeProvider;

        public JiraProvider(IJiraClient client, ITraineeProvider traineeProvider, IProfileContext context)
        {
            _client = client;
            _context = context;
            _traineeProvider = traineeProvider;
        }

        public List<Issue> SearchIssuesByTraineeId(int traineeId)
        {
            var trainee = _traineeProvider.GetTrainee(traineeId);

            // jql query
            var query = new JqlQuery
            {
                Assignee = trainee.User.Login,
                IssueTypes = new List<string> { "Task", "Sub-task", "Bug", "Improvement" },
                StartDate = trainee.Group.StartDate,
                ReleaseDate = trainee.Group.FinishDate,
            };

            // fields to return
            var fields = new List<string>
            {
                "summary",
                "timetracking",
                "assignee",
                "issuetype",
                "status",
            };

            var startAt = 0;
            var maxResults = -1;

            var issues = _client.SearchIssuesByQuery(query, fields, startAt, maxResults).Issues;

            var issuesDataModel = new List<Issue>();
            foreach (var issue in issues)
            {
                issuesDataModel.Add(new Issue
                {
                    Key = issue.Key,
                    Summary = issue.Fields.Summary,
                    Status = issue.Fields.Status.Name,
                    IconUrl = issue.Fields.IssueType.IconUrl,
                    IssueType = issue.Fields.IssueType.Name,
                    OriginalEstimate = issue.Fields.TimeTracking.OriginalEstimateSeconds,
                    TimeSpent = issue.Fields.TimeTracking.TimeSpentSeconds,
                    ColorName = issue.Fields.Status.StatusCategory.ColorName,
                    TraineeId = traineeId
                });
            }

            return issuesDataModel;
        }

        public List<Issue> GetIssuesByTraineeId(int traineeId)
        {
            return _context.Issues.Where(i => i.TraineeId == traineeId).ToList();
        }

        public void SaveIssuesByTraineeId(int traineeId)
        {
            ClearExistingReport(traineeId);
            var issues = SearchIssuesByTraineeId(traineeId);

            _context.Issues.AddRange(issues);
            _context.SaveChanges();
        }

        /// <summary>
        /// If trainee report contains any issues then removing them all.
        /// </summary>
        /// <param name="traineeId">Trainee identifier to find issues.</param>
        private void ClearExistingReport(int traineeId)
        {
            var traineeIssues = _context.Issues.Where(i => i.TraineeId == traineeId).ToList();

            if (traineeIssues.Any())
            {
                _context.Issues.RemoveRange(traineeIssues);
                _context.SaveChanges();
            }
        }
    }
}
