using System.Configuration;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ninject.Modules;
using Profile.BL.Interfaces;
using Profile.BL.Providers;
using Profile.DAL.Context;
using Profile.DAL.Entities;
using Profile.DAL.Identity;
using Profile.JiraRestClient;
using Profile.JiraRestClient.Interfaces;
using Profile.UI.Controllers;
using Profile.UI.Identity;
using Profile.UI.Infrastructure.DetailedInfo;
using Profile.UI.Mappers;
using Profile.UI.Tests.Infrastructure;
using Profile.UI.Tests.Providers;

namespace Profile.UI.Tests.Ninject
{
    public class NinjectTestingModule : NinjectModule
    {
        public override void Load()
        {
            // providers binding
            Bind<IUsersProvider>().To<FakeUsersProvider>();
            Bind<ITraineeProvider>().To<TraineeProvider>();
            Bind<IProfileProvider>().To<ProfileProvider>();
            Bind<ISoftSkillProvider>().To<SoftSkillProvider>();
            Bind<IMainSkillProvider>().To<MainSkillProvider>();
            Bind<ICourseProvider>().To<CourseProvider>();
            Bind<IUniversityProvider>().To<UniversityProvider>();
            Bind<ILanguageProvider>().To<LanguageProvider>();
            Bind<ILinkProvider>().To<LinkProvider>();
            Bind<IJiraProvider>().To<JiraProvider>();
            Bind<IFileProvider>().To<FileProvider>();
            Bind<IProjectProvider>().To<ProjectProvider>();
            Bind<IMentorProvider>().To<MentorProvider>();
            Bind<IMentorReviewProvider>().To<MentorReviewProvider>();
            Bind<IScrumReviewProvider>().To<ScrumReviewProvider>();
            Bind<IScrumMasterProvider>().To<ScrumMasterProvider>();
            Bind<ITraineeReviewProvider>().To<TraineeReviewProvider>();
            Bind<IGroupProvider>().To<GroupProvider>();
            Bind<ISpecializationProvider>().To<SpecializationProvider>();
            Bind<IRoleProvider>().To<RoleProvider>();

            // context binding
            Bind<IProfileContext>()
                .To<ProfileContext>()
                .InScope(x => ProcessingScope.Current)
                .WithConstructorArgument("dbConnectionString", "ProfileDB");

            // Identity
            Bind<IUserStore<User, int>>().To<UserStore>();
            Bind<IRoleStore<Role, int>>().To<RoleStore>();
            Bind<IPermissionStore<Permission, int>>().To<PermissionStore>();
            Bind<UserManager>().To<ConfiguredUserManager>();
            Bind<RoleManager>().ToSelf();
            Bind<IdentityFactoryOptions<UserManager>>()
                .ToMethod(IdentityFactoryOptionsFactory.Create);

            // mappers binding
            Bind<LanguageMapper>().ToSelf().InSingletonScope();
            Bind<SkillMapper>().ToSelf().InSingletonScope();
            Bind<EducationMapper>().ToSelf().InSingletonScope();
            Bind<ProfileMapper>().ToSelf().InSingletonScope();
            Bind<ProjectMapper>().ToSelf().InSingletonScope();
            Bind<MentorMapper>().ToSelf().InSingletonScope();
            Bind<MentorReviewMapper>().ToSelf().InSingletonScope();
            Bind<ScrumReviewMapper>().ToSelf().InSingletonScope();
            Bind<TraineeReviewMapper>().ToSelf().InSingletonScope();
            Bind<GroupMapper>().ToSelf().InSingletonScope();
            Bind<UserMapper>().ToSelf().InSingletonScope();
            Bind<ScrumMasterMapper>().ToSelf().InSingletonScope();
            Bind<HRMapper>().ToSelf().InSingletonScope();
            Bind<RoleMapper>().ToSelf().InSingletonScope();

            // AutoMapperConfiguration binding
            Bind<IMapper>().ToMethod(AutoMapperCreator.GetMapper).InSingletonScope();

            // jira rest client binding
            string jiraUrl = ConfigurationManager.AppSettings["JiraUrl"];
            string jiraUserName = ConfigurationManager.AppSettings["JiraUserName"];
            string jiraPassword = ConfigurationManager.AppSettings["JiraPassword"];

            Bind<IJiraClient>().To<JiraClient>()
                .WithConstructorArgument("jiraUrl", jiraUrl)
                .WithConstructorArgument("jiraUserName", jiraUserName)
                .WithConstructorArgument("jiraPassword", jiraPassword);

            // controllers
            Bind<ManagerController>().ToSelf().InSingletonScope();

            // email service binding
            string userName = ConfigurationManager.AppSettings["SiteEmailAddress"];
            string password = ConfigurationManager.AppSettings["SiteEmailPassword"];
            string smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
            int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);

            Bind<IIdentityMessageService>().To<EmailService>()
                .WithConstructorArgument("userName", userName)
                .WithConstructorArgument("password", password)
                .WithConstructorArgument("smtpHost", smtpHost)
                .WithConstructorArgument("smtpPort", smtpPort);

            Bind<ICurrentUserFactory>().To<FakeUserFactory>();

            Bind<IDetailedInfoBuilder>().To<DetailedInfoBuilder>();
            Bind<ViewDetailsFactory>().ToSelf();
            Bind<EditDetailsFactory>().ToSelf();
        }
    }
}
