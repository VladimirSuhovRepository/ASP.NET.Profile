using System.Collections.Generic;
using System.Linq;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class ProjectProvider : IProjectProvider
    {
        private IProfileContext _context;

        public ProjectProvider(IProfileContext context)
        {
            _context = context;
        }

        public List<Project> GetAll()
        {
            return _context.Projects.ToList();
        }

        public Project GetProject(int id)
        {
            return _context.Projects.Find(id);
        }

        public int GetProjectsCount()
        {
            return _context.Projects.Count();
        }

        public void UpdateProjectDescription(Project project)
        {
            _context.Projects.Attach(project);
            var entry = _context.Entry(project);
            entry.Property(e => e.FullDescription).IsModified = true;
            entry.Property(e => e.ShortDescription).IsModified = true;
            entry.Property(e => e.Status).IsModified = true;

            _context.SaveChanges();
        }

        public Project AddProject(Project newProject)
        {
            var result = _context.Projects.Add(newProject);
            _context.SaveChanges();

            return result;
        }

        public List<Project> GetActive()
        {
            return _context.Projects.Where(p => !p.IsInArchive).ToList();
        }

        public void SendToArchive(Project project)
        {
            project.IsInArchive = true;
            _context.Entry(project).Property(p => p.IsInArchive).IsModified = true;
            _context.Entry(project).Property(p => p.Status).IsModified = true;

            _context.SaveChanges();
        }
    }
}
