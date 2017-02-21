using System.Collections.Generic;
using System.Linq;
using Profile.BL.Interfaces;
using Profile.BL.Models;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class GroupProvider : IGroupProvider
    {
        private IProfileContext _context;

        public GroupProvider(IProfileContext context)
        {
            _context = context;
        }

        public Group GetGroup(int id)
        {
            return _context.Groups.FirstOrDefault(g => g.Id == id);
        }

        public void UpdateGroupDescription(Group group)
        {
            _context.Groups.Attach(group);
            var entry = _context.Entry(group);
            entry.Property(e => e.TeamPurpose).IsModified = true;
            entry.Property(e => e.TeamworkDescription).IsModified = true;

            _context.SaveChanges();
        }

        public Group AddGroup(List<TraineeToMentorId> dataTrainees, Group newGroup, Project toProject)
        {
            _context.Projects.Attach(toProject);

            if (newGroup.ScrumMasterId.HasValue)
            {
                var scrum = _context.ScrumMasters.Find(newGroup.ScrumMasterId);
                scrum.CurrentGroup = newGroup;
            }

            UpdateTrainees(dataTrainees, newGroup);
            toProject.Groups.Add(newGroup);
            _context.SaveChanges();

            return newGroup;
        }

        public Group UpdateGroup(List<TraineeToMentorId> dataTrainees, Group dataGroup)
        {
            var targetGroup = _context.Groups.Find(dataGroup.Id);

            targetGroup.Number = dataGroup.Number;
            targetGroup.StartDate = dataGroup.StartDate;
            targetGroup.FinishDate = dataGroup.FinishDate;

            targetGroup.Project.Name = dataGroup.Project.Name;
            targetGroup.Project.StartDate = dataGroup.Project.StartDate;
            targetGroup.Project.FinishDate = dataGroup.Project.FinishDate;

            if (targetGroup.ScrumMasterId != dataGroup.ScrumMasterId)
            {
                var newScrum = _context.ScrumMasters.Find(dataGroup.ScrumMasterId);

                if (targetGroup.ScrumMaster != null)
                {
                    targetGroup.ScrumMaster.CurrentGroup = null;
                }

                newScrum.CurrentGroup = targetGroup;
                targetGroup.ScrumMaster = newScrum;
            }

            var traineeKeys = dataTrainees.Select(t => t.TraineeId).ToArray();
            var removingTrainees = targetGroup.Trainees.Where(t => !traineeKeys.Contains(t.Id)).ToList();

            foreach (var trainee in removingTrainees)
            {
                targetGroup.Trainees.Remove(trainee);
            }

            UpdateTrainees(dataTrainees, targetGroup);
            _context.SaveChanges();

            return targetGroup;
        }

        private void UpdateTrainees(List<TraineeToMentorId> dataTrainees, Group toGroup)
        {
            var idMentors = dataTrainees.ToDictionary(t => t.TraineeId, d => d.MentorId);
            var trainees = _context.Trainees.Where(t => idMentors.Keys.Contains(t.Id)).ToList();

            foreach (var trainee in trainees)
            {
                if (idMentors[trainee.Id] != 0)
                {
                    trainee.MentorId = idMentors[trainee.Id];
                }

                trainee.Group = toGroup;
            }
        }
    }
}
