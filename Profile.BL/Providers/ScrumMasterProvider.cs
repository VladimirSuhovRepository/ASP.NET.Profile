using System.Collections.Generic;
using System.Linq;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class ScrumMasterProvider : IScrumMasterProvider
    {
        private IProfileContext _context;

        public ScrumMasterProvider(IProfileContext context)
        {
            _context = context;
        }

        public ScrumMaster Get(int id)
        {
            return _context.ScrumMasters.Find(id);
        }

        public List<ScrumMaster> GetAll()
        {
            return _context.ScrumMasters.ToList();
        }
    }
}
