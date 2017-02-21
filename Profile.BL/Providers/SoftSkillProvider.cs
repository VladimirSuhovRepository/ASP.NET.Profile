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
    public class SoftSkillProvider : ISoftSkillProvider
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IProfileContext _context;

        public SoftSkillProvider(IProfileContext profileContext)
        {
            _context = profileContext;
            Logger.Debug("Creating instance of provider");
        }

        public List<SoftSkill> GetSoftSkills(int profileId)
        {
            var softSkills = _context.SoftSkills.Where(s => s.TraineeProfileId == profileId).ToList();

            return softSkills;
        }

        public List<SoftSkill> UpdateSoftSkills(List<SoftSkill> softSkills)
        {
            foreach (var softSkill in softSkills)
            {
                if (softSkill.Id == 0)
                {
                    _context.SoftSkills.Add(softSkill);
                }
                else
                {
                    _context.Entry(softSkill).State = EntityState.Modified;
                }
            }

            _context.SaveChanges();

            return softSkills;
        }

        public void RemoveSoftSkill(int softSkillId)
        {
            if (softSkillId > 0)
            {
                SoftSkill softSkill = new SoftSkill { Id = softSkillId };

                _context.Entry(softSkill).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            else
            {
                Logger.Error($"RemoveSoftSkill: not found softSkill with Id={softSkillId}");
            }
        }
    }
}