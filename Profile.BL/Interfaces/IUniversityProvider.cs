using System;
using System.Collections.Generic;
using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface IUniversityProvider
    {
        List<University> GetUniversities(int profileId);
        List<University> UpdateUniversities(List<University> universities);
        void RemoveUniversity(int universityId);
    }
}
