using System.Collections.Generic;
using Newtonsoft.Json;
using Profile.UI.Attributes;
using Profile.UI.Extentions;
using Profile.UI.Models.Profile;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class EducationsJsonModel
    {
        public EducationsJsonModel()
        {
            Universities = new List<UniversityViewModel>();
            Courses = new List<CourseViewModel>();
        }

        [JsonProperty("profileId")]
        public int ProfileId { get; set; }

        [JsonProperty("universities")]
        public List<UniversityViewModel> Universities { get; set; }

        [JsonProperty("courses")]
        public List<CourseViewModel> Courses { get; set; }

        public void TrimAndUppercaseFirst()
        {
            Universities.ForEach(university => 
            {
                university.EducationalInstitution = university.EducationalInstitution.TrimAndUppercaseFirst();
                university.Specialization = university.Specialization.TrimAndUppercaseFirst();
                university.Specialty = university.Specialty.TrimAndUppercaseFirst();
                university.AdditionalInformation = university.AdditionalInformation.TrimAndUppercaseFirst();
            });

            Courses.ForEach(course => 
            {
                course.CourseName = course.CourseName.TrimAndUppercaseFirst();
                course.EducationalEstablishment = course.EducationalEstablishment.TrimAndUppercaseFirst();
                course.AdditionalInformation = course.AdditionalInformation.TrimAndUppercaseFirst();
            });
        }
    }
}