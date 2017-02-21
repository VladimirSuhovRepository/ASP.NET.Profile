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
    [PermissionAuthorize(Permission = PermissionType.OnlyMentor)]
    public class MentorController : BaseController
    {
        private IMentorProvider _mentorProvider;
        private IUsersProvider _usersProvider;
        private ITraineeProvider _traineeProvider;
        private MentorMapper _mentorMapper;
        private IMentorReviewProvider _reviewProvider;
        private ProjectMapper _projectMapper;
        private MentorReviewMapper _reviewMapper;

        public MentorController(
            IUsersProvider usersProvider,
            ITraineeProvider traineeProvider,
            ProjectMapper projectMapper,
            IMentorReviewProvider reviewProvider,
            IMentorProvider mentorProvider,
            MentorReviewMapper reviewMapper,
            MentorMapper mentorMapper,
            ICurrentUserFactory userFactory)
            : base(userFactory)
        {
            _mentorProvider = mentorProvider;
            _usersProvider = usersProvider;
            _traineeProvider = traineeProvider;
            _mentorMapper = mentorMapper;
            _reviewProvider = reviewProvider;
            _projectMapper = projectMapper;
            _reviewMapper = reviewMapper;
        }

        [HttpGet]
        [OverrideAuthorization]
        [PermissionAuthorize]
        public ActionResult Get(int? id)
        {
            int userId = id.HasValue ? id.Value : CurrentUser.Id.Value;
            var mentor = _mentorProvider.Get(userId);

            mentor.User.Avatar = _usersProvider.GetAvatarByUserIdOrDefault(userId);

            // Map to view model
            var mentorProfileViewModel = _mentorMapper.ToMentorProfileViewModel(mentor);

            return View("View", mentorProfileViewModel);
        }

        [HttpPost]
        public JsonResult Edit(MentorEditRequestJsonModel mentorEditRequestJsonModel)
        {
            // Check if we need to upload new mentor avatar
            if (mentorEditRequestJsonModel.NewAvatar != null)
            {
                _usersProvider.SaveUserAvatar(
                    mentorEditRequestJsonModel.Id,
                    mentorEditRequestJsonModel.NewAvatar.InputStream,
                    mentorEditRequestJsonModel.NewAvatar.ContentType);
            }
            else if (mentorEditRequestJsonModel.IsAvatarDeleting)
            {
                // If we need to delete an existing avatar
                _usersProvider.RemoveAvatarByUserId(mentorEditRequestJsonModel.Id);
            }

            // Update information about mentor
            var mentor = _mentorMapper.FromMentorEditRequestJsonModel(mentorEditRequestJsonModel);
            mentor = _mentorProvider.Update(mentor);
            mentor.User.Avatar = _usersProvider.GetAvatarByUserIdOrDefault(mentor.Id);

            var mentorEditResponseJsonModel = _mentorMapper.ToMentorEditResponseJsonModel(mentor);

            return Json(mentorEditResponseJsonModel);
        }

        [HttpGet]
        public ActionResult GetTrainees()
        {
            var items = _traineeProvider.GetTraineesByMentorId(CurrentUser.Id.Value)
               .Select(_projectMapper.ToProjectTraineeViewModel)
               .OrderBy(t => t.FullName)
               .ToList();

            return View("Trainees", items);
        }

        public ActionResult GetReview(int id)
        {
            var newReview = _reviewProvider.CreateEmptyForTrainee(id);
            var reviewViewModel = _reviewMapper.ToViewModel(newReview);

            return PartialView("_FormReviewPartial", reviewViewModel);
        }

        [HttpPost]
        public ActionResult GetReview(MentorReviewJsonModel reviewJsonModel)
        {
            var reviewModel = _reviewMapper.JsonToBLModel(reviewJsonModel);

            _reviewProvider.AddMentorReview(reviewModel);

            return Content("Отзыв успешно создан");
        }
    }
}