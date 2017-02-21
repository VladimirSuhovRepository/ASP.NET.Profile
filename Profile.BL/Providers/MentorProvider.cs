using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class MentorProvider : IMentorProvider
    {
        private IProfileContext _context;

        public MentorProvider(IProfileContext context)
        {
            _context = context;
        }

        public Mentor Get(int id)
        {
            var mentor = _context.Mentors.Find(id);

            return mentor;
        }

        public List<Mentor> GetAll()
        {
            return _context.Mentors.ToList();
        }

        public List<Mentor> GetMentorsBySpecialization(int specializationId)
        {
            return _context.Mentors.Where(m => m.SpecializationId == specializationId).ToList();
        }

        public Mentor Update(Mentor mentor)
        {
            if (mentor == null) return null;

            var updatedUser = _context.Users.Find(mentor.Id);

            updatedUser.FullName = mentor.User.FullName;
            updatedUser.Contacts = mentor.User.Contacts;
            _context.Entry(updatedUser.Contacts).State = EntityState.Modified;

            _context.SaveChanges();

            return _context.Mentors.Find(mentor.Id);
        }
    }
}
