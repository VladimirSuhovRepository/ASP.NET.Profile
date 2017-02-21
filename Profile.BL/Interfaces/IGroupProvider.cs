using System.Collections.Generic;
using Profile.BL.Models;
using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface IGroupProvider
    {
        Group GetGroup(int id);
        void UpdateGroupDescription(Group group);
        Group AddGroup(List<TraineeToMentorId> dataTrainees, Group newGroup, Project toProject);
        Group UpdateGroup(List<TraineeToMentorId> dataTrainees, Group dataGroup);
    }
}
