using System;
using System.Collections.Generic;
using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface ICourseProvider
    {
        List<Course> GetCourses(int profileId);
        List<Course> UpdateCourses(List<Course> courses);
        void RemoveCourse(int courseId);
    }
}
