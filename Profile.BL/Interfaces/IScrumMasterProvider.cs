using System.Collections.Generic;
using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface IScrumMasterProvider
    {
        ScrumMaster Get(int id);
        List<ScrumMaster> GetAll();
    }
}
