using System.Web.Mvc;
using Profile.BL.Interfaces;
using Profile.DAL.Entities;
using Profile.UI.Identity;
using Profile.UI.Models;
using Profile.UI.Utility;

namespace Profile.UI.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private const string ProjectLabel = "Проект";
        private const string TraineeLabel = "Стажер";

        private ITraineeProvider _traineeProvider; 
        private IProjectProvider _projectProvider;
        private WordTransformer _wordTransformer;

        public HomeController(
            ITraineeProvider traineeProvider, 
            IProjectProvider projectProvider,
            WordTransformer wordTransformer,
            ICurrentUserFactory userFactory)
            : base(userFactory)
        {
            _traineeProvider = traineeProvider;
            _projectProvider = projectProvider;
            _wordTransformer = wordTransformer;
        }
        
        public ActionResult Index()
        {
            var metricsModel = new MetricsViewModel
            {
                ProjectsCount = _projectProvider.GetProjectsCount(),
                TraineesCount = _traineeProvider.GetTraineesCount()
            };
            metricsModel.ProjectsLabel = _wordTransformer.GetNounCase(metricsModel.ProjectsCount, ProjectLabel);
            metricsModel.TraineesLabel = _wordTransformer.GetNounCase(metricsModel.TraineesCount, TraineeLabel);

            return View(metricsModel);
        }

        public ActionResult GetStartPage()
        {
            if (CurrentUser.HasRole(RoleType.ScrumMaster))
            {
                return RedirectToAction("Project", "ScrumMaster");
            }

            if (CurrentUser.HasRole(RoleType.Mentor))
            {
                return RedirectToAction("GetTrainees", "Mentor");
            }

            if (CurrentUser.HasRole(RoleType.Trainee))
            {
                return RedirectToAction("View", "Profile");
            }

            if (CurrentUser.HasRole(RoleType.Manager))
            {
                return RedirectToAction("List", "Trainee");
            }

            // TODO: Add start page for admin
            return RedirectToAction("Index");
        }
    }
}