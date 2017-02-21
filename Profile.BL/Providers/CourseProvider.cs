using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using log4net;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class CourseProvider : ICourseProvider
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IProfileContext _context;

        public CourseProvider(IProfileContext profileContext)
        {
            _context = profileContext;
        }

        public List<Course> GetCourses(int profileId)
        {
            var courses = _context.Courses.Where(e => e.TraineeProfile.Id == profileId).ToList();
            return courses;
        }

        /// <summary>
        /// Attaches the given entities list to the context underlying the set
        /// and updates changed fields in appropriate database entities
        /// that searches by the primary key value or adds new record to
        /// the database context if key value is absent.
        /// 
        /// </summary>
        /// <param name="coursesToUpdate">Entities list to update courses information.</param>
        /// <returns>Updated entities list if exists or null.</returns>
        public List<Course> UpdateCourses(List<Course> coursesToUpdate)
        {
            foreach (var course in coursesToUpdate)
            {
                if (course.Id == 0)
                {
                    _context.Courses.Add(course);
                }
                else
                {
                    _context.Entry(course).State = EntityState.Modified;
                }
            }

            _context.SaveChanges();

            return coursesToUpdate;
        }

        public void RemoveCourse(int courseId)
        {
            if (courseId > 0)
            {
                Course course = new Course { Id = courseId };

                _context.Entry(course).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            else
            {
                Logger.Error($"RemoveCourse: not found course with Id={courseId}");
            }
        }
    }
}
