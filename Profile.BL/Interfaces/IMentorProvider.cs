using System.Collections.Generic;
using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface IMentorProvider
    {
        Mentor Get(int id);
        Mentor Update(Mentor mentor);
        List<Mentor> GetMentorsBySpecialization(int specializationId);
        List<Mentor> GetAll();
    }
}
