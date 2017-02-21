using System.Collections.Generic;
using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface IMainSkillProvider
    {
        List<MainSkill> GetSelectedMainSkills(int profileId);
        List<MainSkill> GetAvailableMainSkills(int profileId);
        void UpdateSelectedMainSkills(int profileId, List<int> mainSkills);
    }
}
