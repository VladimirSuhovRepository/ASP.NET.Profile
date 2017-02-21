using System.Linq;
using System.Web.Mvc;
using Profile.BL.Interfaces;
using Profile.DAL.Entities;
using Profile.UI.Filters;
using Profile.UI.Identity;
using Profile.UI.Mappers;
using Profile.UI.Models;
using Profile.UI.Models.Json;

namespace Profile.UI.Controllers
{
    [PermissionAuthorize(Permission = PermissionType.OnlyScrumMaster)]
    public class ScrumMasterController : BaseController
    {
        private IScrumReviewProvider _scrumReviewProvider;
        private IScrumMasterProvider _scrumMasterProvider;
        private IGroupProvider _groupProvider;
        private IProjectProvider _projectProvider;
        private ScrumReviewMapper _scrumMasterMapper;
        private GroupMapper _groupMapper;
        private ProjectMapper _projectMapper;

        public ScrumMasterController(
            IScrumReviewProvider scrumReviewProvider, 
            IScrumMasterProvider scrumMasterProvider,
            IGroupProvider groupProvider,
            IProjectProvider projectProvider,
            ScrumReviewMapper scrumMasterMapper,
            GroupMapper groupMapper,
            ProjectMapper projectMapper,
            ICurrentUserFactory userFactory)
            : base(userFactory)
        {
            _scrumReviewProvider = scrumReviewProvider;
            _scrumMasterProvider = scrumMasterProvider;
            _scrumMasterMapper = scrumMasterMapper;
            _groupProvider = groupProvider;
            _projectProvider = projectProvider;
            _groupMapper = groupMapper;
            _projectMapper = projectMapper;
        }

        public ActionResult GetReviews()
        {
            var scrum = _scrumMasterProvider.Get(CurrentUser.Id.Value);

            var reviews = scrum.CurrentGroup.Trainees
                .Select(tr => _scrumReviewProvider.GetReviewForTrainee(tr.Id) ?? 
                    _scrumReviewProvider.CreateEmptyForTrainee(tr.Id))
                .OrderBy(tr => tr.ReviewedTrainee.User.FullName).ToList();

            var scrumTraineesVM = _scrumMasterMapper.ReviewToEditViewModel(scrum, reviews);

            return View("Reviews", scrumTraineesVM);
        }

        [HttpPost]
        public ActionResult PostReview(ScrumReviewJsonModel reviewJson)
        {
            var review = _scrumMasterMapper.JsonToBLModel(reviewJson);
            _scrumReviewProvider.AddScrumReview(review);

            return Content("Отзыв успешно оставлен");
        }

        [HttpGet]
        public ActionResult Project()
        {
            var group = _scrumMasterProvider.Get(CurrentUser.Id.Value).CurrentGroup;
            var model = new ScrumProjectViewModel();
            model.Group = _groupMapper.ToGroupViewModel(group);
            model.Project = _projectMapper.ToViewModel(group.Project);
            
            if (model.Project.Status != ProjectStatus.WaitingForDescription)
            {
                return RedirectToAction("View", "Project", new { id = model.Project.Id });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Project(ScrumProjectViewModel model)
        {
            if (CurrentUser.Id.Value == model.Group.ScrumMasterId)
            {
                var project = _projectMapper.FromProjectViewModel(model.Project);
                var group = _groupMapper.FromGroupViewModel(model.Group);
                _projectProvider.UpdateProjectDescription(project);
                _groupProvider.UpdateGroupDescription(group);
            }

            return RedirectToAction("View", "Project", new { id = model.Project.Id });
        }

        public ActionResult Edit()
        {
            var group = _scrumMasterProvider.Get(CurrentUser.Id.Value).CurrentGroup;
            var model = new ScrumProjectViewModel();
            model.Group = _groupMapper.ToGroupViewModel(group);
            model.Project = _projectMapper.ToViewModel(group.Project);

            if (model.Project.Status != ProjectStatus.InProgress)
            {
                return RedirectToAction("View", "Project", new { id = model.Project.Id });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ScrumProjectViewModel model)
        {
            if (CurrentUser.Id.Value == model.Group.ScrumMasterId)
            {
                var project = _projectMapper.FromProjectViewModel(model.Project);
                var group = _groupMapper.FromGroupViewModel(model.Group);
                _projectProvider.UpdateProjectDescription(project);
                _groupProvider.UpdateGroupDescription(group);
            }

            return RedirectToAction("View", "Project", new { id = model.Project.Id });
        }
    }
}