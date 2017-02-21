using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class TraineeProvider : ITraineeProvider
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IProfileContext _context;

        public TraineeProvider(IProfileContext profileContext)
        {
            _context = profileContext;
            Logger.Debug("Creating instance of provider");
        }

        public List<Trainee> GetActualTrainees()
        {
            var trainees = _context.Trainees.Where(t => t.GroupId.HasValue).ToList();

            return trainees;
        }

        public Trainee GetTrainee(int id)
        {
            var trainee = _context.Trainees.FirstOrDefault(t => t.Id == id); 

            return trainee;
        }

        public List<Trainee> GetTraineeGroupmates(int id)
        {
            var trainee = _context.Trainees.Find(id);
            var groupmates = trainee.Group.Trainees
                .Where(t => t.Id != trainee.Id)
                .ToList();

            return groupmates;
        }

        public List<Trainee> GetTraineesByMentorId(int mentorId)
        {
            var trainees = _context.Trainees.Where(t => (t.MentorId == mentorId) && t.GroupId.HasValue).ToList();

            return trainees;
        }
        
        public int GetTraineesCount()
        {
            return _context.Trainees.Count();
        }

        public List<Trainee> GetTraineesByGroupId(int groupId)
        {
            var trainees = _context.Trainees.Where(t => t.GroupId == groupId).ToList();
            return trainees;
        }

        public List<Trainee> GetFreeTraineesBySpecialization(int specializationId)
        {
            var trainees = _context.Trainees.Where(t => t.SpecializationId == specializationId && 
                                                        !t.GroupId.HasValue).ToList();
            return trainees;
        }

        public List<Trainee> GetFreeTrainees()
        {
            return _context.Trainees.Where(t => !t.GroupId.HasValue).ToList();
        }
    }
}
