using System;
using System.Data.Entity;
using System.Reflection;
using log4net;
using Microsoft.AspNet.Identity.EntityFramework;
using Profile.DAL.Entities;
using Profile.DAL.Identity.Entities;

namespace Profile.DAL.Context
{
    public class ProfileContext :
        IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>,
        IProfileContext
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [Obsolete("Use it only for working with EF migrations", true)]
        public ProfileContext() 
            : base("ProfileDB")
        {
        }

        public ProfileContext(string dbConnectionString)
            : base(dbConnectionString)
        {
            Logger.Debug("Creating instance of context");
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<TraineeProfile> TraineeProfiles { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<SoftSkill> SoftSkills { get; set; }
        public DbSet<MainSkill> MainSkills { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<FileData> FileDatas { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<ScrumMaster> ScrumMasters { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigureCascadeDeleting(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureCascadeDeleting(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>()
                .HasOptional(x => x.FileData)
                .WithRequired(x => x.File)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasOptional(x => x.Contacts)
                .WithRequired(x => x.User)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasOptional(x => x.Avatar)
                .WithRequired(x => x.User)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Group>()
                .HasOptional(x => x.ScrumMaster)
                .WithMany(x => x.Groups);

            modelBuilder.Entity<TraineeProfile>()
                .HasMany(x => x.MainSkills)
                .WithMany(x => x.TraineeProfiles);
        }
    }
}
