using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.Models.Json;
using Profile.UI.Models.Profile;

namespace Profile.UI.Mappers
{
    public class EducationMapper
    {
        private IMapper _mapper;

        public EducationMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public EducationsViewModel ToEducationsViewModel(List<University> universities, List<Course> courses)
        {
            var universitiesViewModel = universities.Select(ToUniversityViewModel).ToList();
            var coursesViewModel = courses.Select(ToCourseViewModel).ToList();

            EducationsViewModel educationsViewModel = new EducationsViewModel
            {
                Universities = universitiesViewModel,
                Courses = coursesViewModel
            };

            return educationsViewModel;
        }

        public UniversityViewModel ToUniversityViewModel(University university)
        {
            var universityViewModel = _mapper.Map<University, UniversityViewModel>(university);

            return universityViewModel;
        }

        public CourseViewModel ToCourseViewModel(Course course)
        {
            var courseViewModel = _mapper.Map<Course, CourseViewModel>(course);

            return courseViewModel;
        }

        public University FromUniversityViewModel(int profileId, UniversityViewModel universityViewModel)
        {
            var university = _mapper.Map<UniversityViewModel, University>(universityViewModel);

            university.TraineeProfileId = profileId;

            return university;
        }

        public Course FromCourseViewModel(int profileId, CourseViewModel courseViewModel)
        {
            var course = _mapper.Map<CourseViewModel, Course>(courseViewModel);

            course.TraineeProfileId = profileId;

            return course;
        }

        public EducationsJsonModel ToEducationJsonModel(int profileId, List<University> universities, List<Course> courses)
        {
            var educationsJsonModel = new EducationsJsonModel
            {
                ProfileId = profileId,
                Universities = universities.Select(ToUniversityViewModel).ToList(),
                Courses = courses.Select(ToCourseViewModel).ToList()
            };

            return educationsJsonModel;
        }
    }
}