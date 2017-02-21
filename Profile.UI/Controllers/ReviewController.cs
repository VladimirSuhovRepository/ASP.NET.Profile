using System.Web.Mvc;
using Profile.BL.Interfaces;
using Profile.UI.Mappers;

namespace Profile.UI.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IMentorReviewProvider _mentorReviewProvider;
        private readonly IMentorProvider _mentorProvider;
        private readonly IProfileProvider _profileProvider;
        private readonly IScrumReviewProvider _scrumReviewProvider;
        private readonly ITraineeReviewProvider _traineeReviewProvider; 
        private readonly ProfileMapper _profileMapper;
        private readonly MentorReviewMapper _mentorReviewMapper;
        private readonly ScrumReviewMapper _scrumMasterMapper;
        private readonly TraineeReviewMapper _traineeReviewMapper;

        public ReviewController(
            IMentorReviewProvider mentorReviewProvider,
            IMentorProvider mentorProvider,
            IProfileProvider profileProvider,
            IScrumReviewProvider scrumReviewProvider,
            ITraineeReviewProvider traineeReviewProvider,
            ProfileMapper profileMapper,
            ScrumReviewMapper scrumMasterMapper,
            MentorReviewMapper mentorReviewMapper,
            TraineeReviewMapper traineeReviewMapper)
        {
            _mentorReviewProvider = mentorReviewProvider;
            _mentorProvider = mentorProvider;
            _profileProvider = profileProvider;
            _scrumReviewProvider = scrumReviewProvider;
            _traineeReviewProvider = traineeReviewProvider;

            _profileMapper = profileMapper;
            _scrumMasterMapper = scrumMasterMapper;
            _mentorReviewMapper = mentorReviewMapper;
            _traineeReviewMapper = traineeReviewMapper;
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            var profile = _profileProvider.GetProfileByTraineeId(id);

            var profileReviewViewModel = _profileMapper.ToProfileReviewViewModel(
                profile,
                _profileProvider.GetTraineeRating(id),
                _profileProvider.IsTraineeHavingMentorReview(id),
                _profileProvider.IsTraineeHavingScrumReview(id),
                _profileProvider.IsTraineeHavingTeamReview(id));

            if (!profileReviewViewModel.HasReviews)
            {
                return new HttpNotFoundResult();
            }

            if (profileReviewViewModel.IsTraineeHavingTeamReview)
            {
                var grades = _traineeReviewProvider.GetTeamGradesOnTrainee(id);
                var abilities = _traineeReviewProvider.GetAbilities();

                profileReviewViewModel.RenderingReview = _traineeReviewMapper
                    .ToLinkedTeamReviewViewModel(abilities, grades);
            }
            else if (profileReviewViewModel.IsTraineeHavingMentorReview)
            {
                var review = _mentorReviewProvider.GetMentorReviewForTrainee(id);

                profileReviewViewModel.RenderingReview = _mentorReviewMapper
                    .ToLinkedMentorReviewViewModel(review);
            }
            else if (profileReviewViewModel.IsTraineeHavingScrumReview)
            {
                var review = _scrumReviewProvider.GetReviewForTrainee(id);

                profileReviewViewModel.RenderingReview = _scrumMasterMapper.ReviewToLinkedViewModel(review);
            }

            return View(profileReviewViewModel);
        }

        [HttpGet]
        public PartialViewResult GetMentorReview(int id)
        {
            var mentorReview = _mentorReviewProvider.GetMentorReviewForTrainee(id);
            var mentorReviewViewModel = _mentorReviewMapper
                .ToLinkedMentorReviewViewModel(mentorReview);

            return PartialView("_PartialProfileMentorReview", mentorReviewViewModel);
        }

        [HttpGet]
        public ActionResult GetScrumReview(int id)
        {
            var review = _scrumReviewProvider.GetReviewForTrainee(id);
            var scrumReviewViewModel = _scrumMasterMapper.ReviewToViewModel(review);

            return PartialView("_ScrumReviewPartial", scrumReviewViewModel);
        }

        [HttpGet]
        public PartialViewResult GetTeamReview(int id)
        {
            var grades = _traineeReviewProvider.GetTeamGradesOnTrainee(id);
            var abilities = _traineeReviewProvider.GetAbilities();

            var teamReviewViewModel = _traineeReviewMapper
                .ToLinkedTeamReviewViewModel(abilities, grades);

            return PartialView("_PartialProfileTeamReview", teamReviewViewModel);
        }
    }
}