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
    public class UniversityProvider : IUniversityProvider
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IProfileContext _context;

        public UniversityProvider(IProfileContext profileContext)
        {
            _context = profileContext;
        }

        public List<University> GetUniversities(int profileId)
        {
            var universities = _context.Universities.Where(e => e.TraineeProfile.Id == profileId).ToList();
            return universities;
        }

        /// <summary>
        /// Attaches the given entities list to the context underlying the set
        /// and updates changed fields in appropriate database entities
        /// that searches by the primary key value or adds new record to
        /// the database context if key value is absent.
        /// 
        /// </summary>
        /// <param name="universitiesToUpdate">Entities list to update universities information.</param>
        /// <returns>Updated entities list if exists or null.</returns>
        public List<University> UpdateUniversities(List<University> universitiesToUpdate)
        {
            foreach (var university in universitiesToUpdate)
            {
                if (university.Id == 0)
                {
                    _context.Universities.Add(university);
                }
                else
                {
                    _context.Entry(university).State = EntityState.Modified;
                }
            }

            _context.SaveChanges();

            return universitiesToUpdate;
        }

        public void RemoveUniversity(int universityId)
        {
            if (universityId > 0)
            {
                University university = new University { Id = universityId };

                _context.Entry(university).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            else
            {
                Logger.Error($"RemoveUniversity: not found university with Id={universityId}");
            }
        }
    }
}
