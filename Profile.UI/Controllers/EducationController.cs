using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using log4net;
using Profile.BL.Interfaces;
using Profile.UI.Mappers;
using Profile.UI.Models.Json;
using Profile.UI.Models.Profile;

namespace Profile.UI.Controllers
{
    public class EducationController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IUniversityProvider _universityProvider;
        private ICourseProvider _courseProvider;
        private EducationMapper _educationMapper;

        public EducationController(IUniversityProvider universityProvider, ICourseProvider courseProvider, EducationMapper educationMapper)
        {
            _universityProvider = universityProvider;
            _courseProvider = courseProvider;
            _educationMapper = educationMapper;
        }

        #region Education

        [HttpGet]
        public PartialViewResult Educations(int profileId)
        {
            var universities = _universityProvider.GetUniversities(profileId);
            var courses = _courseProvider.GetCourses(profileId);

            EducationsViewModel educationsViewModel = _educationMapper.ToEducationsViewModel(universities, courses);

            return PartialView("_EditPartialEducations", educationsViewModel);
        }

        [HttpPost]
        public JsonResult EditEducations(EducationsJsonModel educationsJson)
        {
            educationsJson.TrimAndUppercaseFirst();

            var universities = educationsJson.Universities
                .Select(university => _educationMapper.FromUniversityViewModel(educationsJson.ProfileId, university))
                .ToList();

            var courses = educationsJson.Courses
                .Select(course => _educationMapper.FromCourseViewModel(educationsJson.ProfileId, course))
                .ToList();

            var updatedUniversities = _universityProvider.UpdateUniversities(universities);
            var updatedCourses = _courseProvider.UpdateCourses(courses);

            var updatedEducationsJson = _educationMapper.ToEducationJsonModel(educationsJson.ProfileId, universities, courses);

            return Json(updatedEducationsJson);
        }

        #endregion

        #region University

        [HttpGet]
        public PartialViewResult Universities(int profileId)
        {
            var universities = _universityProvider.GetUniversities(profileId);

            var universitiesViewModel = universities.Select(_educationMapper.ToUniversityViewModel).ToList();

            return PartialView("_PartialUniversities", universitiesViewModel);
        }

        [HttpDelete]
        public void RemoveUniversity(UniversityViewModel university)
        {
            _universityProvider.RemoveUniversity(university.Id);
        }

        #endregion

        #region Course

        [HttpGet]
        public PartialViewResult Courses(int profileId)
        {
            var courses = _courseProvider.GetCourses(profileId);

            var coursesViewModel = courses.Select(_educationMapper.ToCourseViewModel).ToList();

            return PartialView("_PartialCourses", coursesViewModel);
        }

        [HttpDelete]
        public void RemoveCourse(CourseViewModel course)
        {
            _courseProvider.RemoveCourse(course.Id);
        }

        #endregion
    }
}