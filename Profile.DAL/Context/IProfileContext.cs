using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Profile.DAL.Entities;

namespace Profile.DAL.Context
{
    public interface IProfileContext : IDisposable
    {
        IDbSet<Role> Roles { get; set; }
        IDbSet<User> Users { get; set; }
        DbSet<Permission> Permissions { get; set; }

        DbSet<Trainee> Trainees { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<Specialization> Specializations { get; set; }
        DbSet<TraineeProfile> TraineeProfiles { get; set; }
        DbSet<University> Universities { get; set; }
        DbSet<Course> Courses { get; set; }
        DbSet<Language> Languages { get; set; }
        DbSet<SoftSkill> SoftSkills { get; set; }
        DbSet<File> Files { get; set; }
        DbSet<FileData> FileDatas { get; set; }
        DbSet<Link> Links { get; set; }
        DbSet<MainSkill> MainSkills { get; set; }
        DbSet<Issue> Issues { get; set; }
        DbSet<Mentor> Mentors { get; set; }
        DbSet<Contacts> Contacts { get; set; }
        DbSet<Avatar> Avatars { get; set; }
        DbSet<Skill> Skills { get; set; }
        DbSet<Grade> Grades { get; set; }
        DbSet<ScrumMaster> ScrumMasters { get; set; }
        DbSet<Review> Reviews { get; set; }

        DbEntityEntry Entry(object entry);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        int SaveChanges();
    }
}
