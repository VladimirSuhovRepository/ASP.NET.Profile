using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ninject;
using Ninject.Web.Common;
using Profile.BL.Interfaces;
using Profile.BL.Providers;
using Profile.DAL.Context;
using Profile.DAL.Entities;
using Profile.DAL.Identity;
using Profile.JiraRestClient;
using Profile.JiraRestClient.Interfaces;
using Profile.UI.Identity;
using Profile.UI.Infrastructure.DetailedInfo;
using Profile.UI.Mappers;
using Profile.UI.Utility;

namespace Profile.UI.Dependencies
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            _kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            // providers binding
            _kernel.Bind<IUsersProvider>().To<UsersProvider>();
            _kernel.Bind<ITraineeProvider>().To<TraineeProvider>();
            _kernel.Bind<IProfileProvider>().To<ProfileProvider>();
            _kernel.Bind<ISoftSkillProvider>().To<SoftSkillProvider>();
            _kernel.Bind<IMainSkillProvider>().To<MainSkillProvider>();
            _kernel.Bind<ICourseProvider>().To<CourseProvider>();
            _kernel.Bind<IUniversityProvider>().To<UniversityProvider>();
            _kernel.Bind<ILanguageProvider>().To<LanguageProvider>();
            _kernel.Bind<ILinkProvider>().To<LinkProvider>();
            _kernel.Bind<IJiraProvider>().To<JiraProvider>();
            _kernel.Bind<IFileProvider>().To<FileProvider>();
            _kernel.Bind<IProjectProvider>().To<ProjectProvider>();
            _kernel.Bind<IMentorProvider>().To<MentorProvider>();
            _kernel.Bind<IMentorReviewProvider>().To<MentorReviewProvider>();
            _kernel.Bind<IScrumReviewProvider>().To<ScrumReviewProvider>();
            _kernel.Bind<IScrumMasterProvider>().To<ScrumMasterProvider>();
            _kernel.Bind<ITraineeReviewProvider>().To<TraineeReviewProvider>();
            _kernel.Bind<IGroupProvider>().To<GroupProvider>();
            _kernel.Bind<ISpecializationProvider>().To<SpecializationProvider>();
            _kernel.Bind<IRoleProvider>().To<RoleProvider>();

            // context binding
            _kernel.Bind<IProfileContext>().To<ProfileContext>().InRequestScope().WithConstructorArgument("dbConnectionString", "ProfileDB");
            _kernel.Bind<ProfileContext>().ToSelf().WithConstructorArgument("dbConnectionString", "ProfileDB");

            // Identity
            _kernel.Bind<IUserStore<User, int>>().To<UserStore>();
            _kernel.Bind<IRoleStore<Role, int>>().To<RoleStore>();
            _kernel.Bind<IPermissionStore<Permission, int>>().To<PermissionStore>();
            _kernel.Bind<UserManager>().To<ConfiguredUserManager>();
            _kernel.Bind<RoleManager>().ToSelf();
            _kernel.Bind<IdentityFactoryOptions<UserManager>>()
                .ToMethod(IdentityFactoryOptionsFactory.Create);

            // mappers binding
            _kernel.Bind<LanguageMapper>().ToSelf().InSingletonScope();
            _kernel.Bind<SkillMapper>().ToSelf().InSingletonScope();
            _kernel.Bind<EducationMapper>().ToSelf().InSingletonScope();
            _kernel.Bind<ProfileMapper>().ToSelf().InSingletonScope();
            _kernel.Bind<ProjectMapper>().ToSelf().InSingletonScope();
            _kernel.Bind<MentorMapper>().ToSelf().InSingletonScope();
            _kernel.Bind<MentorReviewMapper>().ToSelf().InSingletonScope();
            _kernel.Bind<ScrumReviewMapper>().ToSelf().InSingletonScope();
            _kernel.Bind<TraineeReviewMapper>().ToSelf().InSingletonScope();
            _kernel.Bind<GroupMapper>().ToSelf().InSingletonScope();
            _kernel.Bind<UserMapper>().ToSelf().InSingletonScope();
            _kernel.Bind<ScrumMasterMapper>().ToSelf().InSingletonScope();
            _kernel.Bind<HRMapper>().ToSelf().InSingletonScope();
            _kernel.Bind<RoleMapper>().ToSelf().InSingletonScope();

            _kernel.Bind<WordTransformer>().ToSelf().InSingletonScope();
            
            // AutoMapperConfiguration binding
            _kernel.Bind<IMapper>().ToMethod(AutoMapperCreator.GetMapper).InSingletonScope();

            // jira rest client binding
            string jiraUrl = ConfigurationManager.AppSettings["JiraUrl"];
            string jiraUserName = ConfigurationManager.AppSettings["JiraUserName"];
            string jiraPassword = ConfigurationManager.AppSettings["JiraPassword"];

            _kernel.Bind<IJiraClient>().To<JiraClient>()
                .WithConstructorArgument("jiraUrl", jiraUrl)
                .WithConstructorArgument("jiraUserName", jiraUserName)
                .WithConstructorArgument("jiraPassword", jiraPassword);

            // email service binding
            string userName = ConfigurationManager.AppSettings["SiteEmailAddress"];
            string password = ConfigurationManager.AppSettings["SiteEmailPassword"];
            string smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
            int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);

            _kernel.Bind<IIdentityMessageService>().To<EmailService>()
                .WithConstructorArgument("userName", userName)
                .WithConstructorArgument("password", password)
                .WithConstructorArgument("smtpHost", smtpHost)
                .WithConstructorArgument("smtpPort", smtpPort);

            _kernel.Bind<ICurrentUserFactory>().To<CurrentUserFactory>();

            _kernel.Bind<IDetailedInfoBuilder>().To<DetailedInfoBuilder>();
            _kernel.Bind<ViewDetailsFactory>().ToSelf();
            _kernel.Bind<EditDetailsFactory>().ToSelf();
        }
    }
}
