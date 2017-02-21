using System.Collections.Generic;
using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface ISpecializationProvider
    {
        List<Specialization> GetAll();
    }
}
