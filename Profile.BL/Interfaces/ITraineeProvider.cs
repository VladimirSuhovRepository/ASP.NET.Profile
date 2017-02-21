using System.Collections.Generic;
using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface ITraineeProvider
    {
        List<Trainee> GetActualTrainees();
        Trainee GetTrainee(int id);
        List<Trainee> GetTraineesByMentorId(int mentorId);
        List<Trainee> GetTraineeGroupmates(int id);
        int GetTraineesCount();
        List<Trainee> GetTraineesByGroupId(int groupId);
        List<Trainee> GetFreeTraineesBySpecialization(int specializationId);
        List<Trainee> GetFreeTrainees();
    }
}
