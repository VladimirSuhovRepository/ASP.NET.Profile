using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class MainSkillProvider : IMainSkillProvider
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IProfileContext _context;

        public MainSkillProvider(IProfileContext profileContext)
        {
            _context = profileContext;
            Logger.Debug("Creating instance of provider");
        }

        public List<MainSkill> GetSelectedMainSkills(int profileId)
        {
            var mainSkills = _context.TraineeProfiles.Find(profileId).MainSkills.ToList();

            return mainSkills;
        }

        public List<MainSkill> GetAvailableMainSkills(int profileId)
        {
            List<MainSkill> mainSkills = _context.MainSkills.Where(s => s.Specialization.Trainees.Any(t => t.Id == profileId)).ToList();

            return mainSkills;
        }

        public void UpdateSelectedMainSkills(int profileId, List<int> mainSkillsId)
        {
            TraineeProfile profile = _context.TraineeProfiles.Find(profileId);
            List<MainSkill> availableMainSkills = GetAvailableMainSkills(profileId);

            if (mainSkillsId == null || mainSkillsId.Count == 0)
            {
                profile.MainSkills.Clear();
            }
            else
            {
                var deletingSkills = profile.MainSkills.Where(skill => !mainSkillsId.Contains(skill.Id)).ToList();

                foreach (var skill in deletingSkills)
                {
                    profile.MainSkills.Remove(skill);
                }

                var addingSkills = availableMainSkills.Where(skill => mainSkillsId.Contains(skill.Id) && !profile.MainSkills.Any(s => s.Id == skill.Id));

                foreach (var skill in addingSkills)
                {
                    profile.MainSkills.Add(skill);
                }
            }

            _context.SaveChanges();
        }
    }
}
