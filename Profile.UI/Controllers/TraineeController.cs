using System.Linq;
using System.Web.Mvc;
using Profile.BL.Interfaces;
using Profile.DAL.Entities;
using Profile.UI.Filters;
using Profile.UI.Identity;
using Profile.UI.Mappers;
using Profile.UI.Models.Json;

namespace Profile.UI.Controllers
{
    [PermissionAuthorize(Permission = PermissionType.OnlyTrainee)]
    public class TraineeController : BaseController
    {
        private ITraineeProvider _traineeProvider;
        private ITraineeReviewProvider _traineeReviewProvider;
        private IUsersProvider _usersProvider;
        private TraineeMapper _traineeMapper;
        private TraineeReviewMapper _traineeReviewMapper;

        public TraineeController(
            ITraineeProvider traineeProvider,
            ITraineeReviewProvider traineeReviewProvider,
            IUsersProvider usersProvider,
            TraineeMapper traineeMapper,
            TraineeReviewMapper traineeReviewMapper,
            ICurrentUserFactory userFactory)
            : base(userFactory)
        {
            _traineeProvider = traineeProvider;
            _traineeMapper = traineeMapper;
            _usersProvider = usersProvider;
            _traineeReviewProvider = traineeReviewProvider;
            _traineeReviewMapper = traineeReviewMapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult List()
        {
            var traineesView = _traineeProvider.GetActualTrainees()
              .Select(_traineeMapper.ToTraineeViewModel)
              .ToList();

            return View(traineesView);
        }
        
        [HttpGet]
        public ActionResult GroupmatesList()
        {
            int traineeId = CurrentUser.Id.Value;
            var traineesView = _traineeProvider.GetTraineeGroupmates(traineeId)
                .Select(t => _traineeMapper.ToTraineeViewModel(t, traineeId))
                .OrderBy(t => t.FullName)
                .ToList();

            return View(traineesView);
        }
        
        [HttpGet]
        public PartialViewResult CreateReview(int id)
        {
            var newReview = _traineeReviewProvider.CreateEmptyForTrainee(id, CurrentUser.Id.Value);
            var abilities = _traineeReviewProvider.GetAbilities();
            var reviewViewModel = _traineeReviewMapper.ToViewModel(newReview, abilities);

            return PartialView("~/Views/Trainee/_PartialReviewModal.cshtml", reviewViewModel);
        }
        
        [HttpPost]
        public ActionResult CreateReview(TraineeReviewJsonModel reviewJsonModel)
        {
            var review = _traineeReviewMapper.FromJsonModel(reviewJsonModel);

            _traineeReviewProvider.AddTraineeReview(review);

            return Content("Отзыв создан успешно!");
        }
    }
}
