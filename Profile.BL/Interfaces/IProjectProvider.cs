using System.Collections.Generic;
using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface IProjectProvider
    {
        List<Project> GetAll();
        List<Project> GetActive();
        Project GetProject(int id);
        int GetProjectsCount();
        void UpdateProjectDescription(Project project);
        Project AddProject(Project newProject);
        void SendToArchive(Project project);
    }
}
