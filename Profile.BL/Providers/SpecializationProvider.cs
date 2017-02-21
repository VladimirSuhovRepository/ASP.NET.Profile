using System.Collections.Generic;
using System.Linq;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class SpecializationProvider : ISpecializationProvider
    {
        private IProfileContext _context;

        public SpecializationProvider(IProfileContext context)
        {
            _context = context;
        }

        public List<Specialization> GetAll()
        {
            return _context.Specializations.ToList();
        }
    }
}
