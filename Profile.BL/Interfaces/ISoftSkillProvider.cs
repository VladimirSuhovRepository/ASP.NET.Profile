using System;
using System.Collections.Generic;
using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface ISoftSkillProvider
    {
        List<SoftSkill> GetSoftSkills(int profileId);
        List<SoftSkill> UpdateSoftSkills(List<SoftSkill> softSkills);
        void RemoveSoftSkill(int softSkillId);
    }
}
