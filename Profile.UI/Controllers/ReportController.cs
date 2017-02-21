using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Profile.BL.Interfaces;
using Profile.UI.Mappers;
using Profile.UI.Models.Report;

namespace Profile.UI.Controllers
{
    public class ReportController : Controller
    {
        private IJiraProvider _jiraProvider;
        private IProfileProvider _profileProvider;
        private ProfileMapper _profileMapper;

        public ReportController(IJiraProvider jiraProvider, IProfileProvider profileProvider, ProfileMapper profileMapper)
        {
            _jiraProvider = jiraProvider;
            _profileProvider = profileProvider;
            _profileMapper = profileMapper;
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            var profile = _profileProvider.GetProfileByTraineeId(id);
            var issues = _jiraProvider.GetIssuesByTraineeId(id);
            var issuesViewModel = new List<IssueViewModel>();

            if (issues.Any())
            {
                foreach (var issue in issues)
                {
                    issuesViewModel.Add(new IssueViewModel
                    {
                        Key = issue.Key,
                        Summary = issue.Summary,
                        Status = issue.Status,
                        IconUrl = issue.IconUrl,
                        IssueType = issue.IssueType,
                        OriginalEstimate = issue.OriginalEstimate,
                        TimeSpent = issue.TimeSpent,
                        ColorName = issue.ColorName
                    });
                }
            }

            var profileViewModel = _profileMapper.ToProfileViewModel(profile, _profileProvider.GetTraineeRating(id));
            profileViewModel.Issues = issuesViewModel;

            ViewBag.TotalEstimatedTime = issuesViewModel.Sum(s => s.OriginalEstimate);
            ViewBag.TotalLoggedTime = issuesViewModel.Sum(s => s.TimeSpent);

            return View(profileViewModel);
        }

        [HttpGet]
        public ActionResult List(int id)
        {
            var issues = _jiraProvider.SearchIssuesByTraineeId(id);
            var issuesViewModel = new List<IssueViewModel>();

            foreach (var issue in issues)
            {
                issuesViewModel.Add(new IssueViewModel
                {
                    Key = issue.Key,
                    Summary = issue.Summary,
                    Status = issue.Status,
                    IconUrl = issue.IconUrl,
                    IssueType = issue.IssueType,
                    OriginalEstimate = issue.OriginalEstimate,
                    TimeSpent = issue.TimeSpent,
                    ColorName = issue.ColorName
                });
            }

            ViewBag.TotalEstimatedTime = issuesViewModel.Sum(s => s.OriginalEstimate);
            ViewBag.TotalLoggedTime = issuesViewModel.Sum(s => s.TimeSpent);

            return PartialView(issuesViewModel);
        }

        [HttpPost]
        public void Save(int id)
        {
            _jiraProvider.SaveIssuesByTraineeId(id);
        }
    }
}