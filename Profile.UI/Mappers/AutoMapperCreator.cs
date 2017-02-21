using System.Linq;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Ninject.Activation;
using Profile.BL.Models;
using Profile.DAL.Entities;
using Profile.UI.Models;
using Profile.UI.Models.Json;
using Profile.UI.Models.Manager;
using Profile.UI.Models.Profile;
using Profile.UI.Models.Project;
using Profile.UI.Models.Review;
using Profile.UI.Models.User;

namespace Profile.UI.Mappers
{
    public class AutoMapperCreator
    {
        public static IMapper GetMapper(IContext context)
        {
            MapperConfiguration config = new MapperConfiguration(RegisterMappings);
            IMapper mapper = config.CreateMapper();
            return mapper;
        }

        private static void RegisterMappings(IMapperConfiguration config)
        {
            config.CreateMap<Language, LanguageViewModel>();
            config.CreateMap<LanguageViewModel, Language>();

            config.CreateMap<University, UniversityViewModel>();
            config.CreateMap<UniversityViewModel, University>();

            config.CreateMap<Course, CourseViewModel>();
            config.CreateMap<CourseViewModel, Course>();

            config.CreateMap<MainSkill, MainSkillViewModel>();

            config.CreateMap<SoftSkill, SoftSkillJson>();
            config.CreateMap<SoftSkill, SoftSkillViewModel>();
            config.CreateMap<SoftSkillJson, SoftSkill>();
            config.CreateMap<Contacts, ContactsViewModel>();

            config.CreateMap<TraineeProfile, PositionViewModel>();
            config.CreateMap<TraineeProfile, JobViewModel>();
            config.CreateMap<TraineeProfile, QualificationViewModel>();

            config.CreateMap<File, FileViewModel>();
            config.CreateMap<Link, LinkViewModel>();

            config.CreateMap<ProfileMainInfoJson, TraineeProfile>();
            config.CreateMap<TraineeProfile, ProfileMainInfoJson>();
            config.CreateMap<Contacts, ProfileMainInfoJson>();

            config.CreateMap<ProfileQualificationJson, TraineeProfile>();
            config.CreateMap<TraineeProfile, ProfileQualificationJson>();

            config.CreateMap<Skill, SkillViewModel>();
            config.CreateMap<SkillViewModel, Skill>();

            config.CreateMap<Trainee, TraineeViewModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization.Name))
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Group.Project.Name));

            config.CreateMap<Grade, GradeViewModel>();
            config.CreateMap<GradeViewModel, Grade>();

            config.CreateMap<Review, MentorReviewViewModel>()
                .ForMember(dest => dest.MentorId, opt => opt.MapFrom(src => src.ReviewerId));

            config.CreateMap<MentorReviewJsonModel, Review>()
                .ForMember(dest => dest.ReviewerId, opt => opt.MapFrom(src => src.MentorId));

            config.CreateMap<Review, ScrumReviewEditViewModel>()
                .ForMember(
                    dest => dest.Comment, 
                    opt => opt.MapFrom(src => src.Grades.FirstOrDefault().Comment ?? string.Empty))
                .ForMember(
                    dest => dest.Mark,
                    opt => opt.MapFrom(src => src.Grades.FirstOrDefault().Mark))
                .ForMember(dest => dest.IsReviewed, opt => opt.MapFrom(src => src.Id > 0));

            config.CreateMap<ScrumReviewJsonModel, Review>()
                .ForMember(dest => dest.ReviewerId, opt => opt.MapFrom(src => src.ScrumMasterId));

            config.CreateMap<Review, ScrumReviewViewModel>()
                .ForMember(
                    dest => dest.ScrumMasterName, 
                    opt => opt.MapFrom(src => src.Reviewer.FullName))
                .ForMember(
                    dest => dest.Comment,
                    opt => opt.MapFrom(src => src.Grades.FirstOrDefault().Comment ?? string.Empty))
                .ForMember(
                    dest => dest.Mark,
                    opt => opt.MapFrom(src => src.Grades.FirstOrDefault().Mark))
                .Include<Review, LinkedScrumReviewViewModel>();

            config.CreateMap<Review, LinkedScrumReviewViewModel>();

            config.CreateMap<Grade, TeammateSkillGradeViewModel>();
            config.CreateMap<Skill, TeamReviewedSkillViewModel>();

            config.CreateMap<Review, TraineeReviewViewModel>();
            config.CreateMap<TraineeReviewJsonModel, Review>();

            config.CreateMap<Project, ProjectViewModel>()
                .Include<Project, ManagerProjectViewModel>()
                .Include<Project, ProjectDescriptionViewModel>();

            config.CreateMap<Project, ManagerProjectViewModel>();

            config.CreateMap<Group, GroupViewModel>()
                .Include<Group, GroupDescriptionViewModel>();

            config.CreateMap<GroupViewModel, Group>();

            config.CreateMap<Trainee, ManagerTraineeJsonModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization.Abbreviation))
                .ForMember(dest => dest.MentorName, opt => opt.MapFrom(src => src.Mentor.User.FullName));

            config.CreateMap<Project, ProjectDescriptionViewModel>();
            config.CreateMap<Group, GroupDescriptionViewModel>();
            config.CreateMap<Trainee, GroupTraineeViewModel>();
            config.CreateMap<ScrumMaster, GroupScrumMasterViewModel>();

            config.CreateMap<IdentityResult, ResetPasswordResponseJsonModel>()
                .ForMember(
                    dest => dest.Error, 
                    opt => opt.MapFrom(src => string.Join(", ", src.Errors)));

            config.CreateMap<Project, ProjectViewModel>();
            config.CreateMap<ProjectViewModel, Project>();
            config.CreateMap<User, UserNameCommonModel>();
            config.CreateMap<Specialization, SpecializationNameViewModel>();
            config.CreateMap<ScrumMaster, ScrumHasProjectViewModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName));

            config.CreateMap<TeamPostJsonModel, Group>()
                .ForMember(dest => dest.Trainees, opt => opt.Ignore())
                .ForMember(dest => dest.ScrumMasterId, opt => opt.MapFrom(src => src.ScrumId));

            config.CreateMap<TeamProjectPostJsonModel, Project>();
            config.CreateMap<ManagerTraineeJsonModel, TraineeToMentorId>()
                .ForMember(dest => dest.TraineeId, opt => opt.MapFrom(src => src.Id));

            config.CreateMap<Group, GroupWithTraineesViewModel>()
                .ForMember(dest => dest.Trainees, opt => opt.Ignore());

            config.CreateMap<Mentor, UserBySpecialization>()
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization.Name))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName));

            config.CreateMap<User, UserProfile>();
            config.CreateMap<Contacts, UserContacts>();

            config.CreateMap<Mentor, MentorInfo>();
            config.CreateMap<Mentor, DetailedMentorInfo>();

            config.CreateMap<Contacts, HRInfo>();
            config.CreateMap<Contacts, DetailedHRInfo>();

            config.CreateMap<ScrumMaster, ScrumMasterInfo>()
                .ForMember(dest => dest.ProjectName, 
                           opt => opt.MapFrom(src => src.CurrentGroup.Project.Name))
                .Include<ScrumMaster, DetailedScrumMasterInfo>();

            config.CreateMap<ScrumMaster, DetailedScrumMasterInfo>();

            config.CreateMap<User, NewUserViewModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Contacts.Email));
        }
    }
}