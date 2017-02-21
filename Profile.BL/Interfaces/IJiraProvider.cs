using System;
using System.Collections.Generic;
using Profile.DAL.Entities;
using Profile.JiraRestClient.Entities.Searching;

namespace Profile.BL.Interfaces
{
    public interface IJiraProvider
    {
        List<Issue> SearchIssuesByTraineeId(int traineeId);
        List<Issue> GetIssuesByTraineeId(int traineeId);
        void SaveIssuesByTraineeId(int traineeId);
    }
}
