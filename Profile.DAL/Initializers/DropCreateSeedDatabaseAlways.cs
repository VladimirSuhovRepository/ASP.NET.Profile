using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using Profile.DAL.Context;
using Profile.DAL.Entities;
using Profile.DAL.Identity;

namespace Profile.DAL.Initializers
{
    public class DropCreateSeedDatabaseAlways : DropCreateDatabaseAlways<ProfileContext>
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Random Random = new Random();

        private UserManager _userManager;
        private RoleManager _roleManager;

        public DropCreateSeedDatabaseAlways()
        {
            Logger.Debug("Creating instance of initializer");
        }

        protected override void Seed(ProfileContext context)
        {
            Logger.Debug("Running Seed() method in initializer");

            _userManager = new UserManager(new UserStore(context), null, null);
            _roleManager = new RoleManager(new RoleStore(context));

            FillDatabaseWithTestValues(context);
        }

        private void FillDatabaseWithTestValues(ProfileContext context)
        {
            Logger.Debug("Started filling database with test values");

            AddRoles(context);
            AddPermissions(context);

            AddProjects(context);

            // AddReleases(context);
            AddGroups(context);

            AddSpecializations(context);

            AddNewUsers(context);
            AddManager(context);
            AddAdmin(context);
            AddHRs(context);

            AddCurrentProjectData(context);

            // AddMainSkills(context);
            // AddTrainees(context);
            AddTraineeProfiles(context);

            // AddLanguages(context);
            // AddUniversities(context);
            // AddCourses(context);
            AddSoftSkills(context);
            AddAbilities(context);

            AddMentors(context);
            AddScrumMasters(context);
            AddScrumMentors(context);

            // AddArtefacts(context);
            AddMentorSpecializationAndTrainees(context);
            AddScrumGroup(context);

            AddMentorReviews(context);
            AddScrumReviews(context);
            AddTeamReviews(context);
            RemoveReviews(context);
            AddEmptyScrums(context);
            Logger.Debug("Database was successfully filled with test values");
        }

        private void AddScrumMentors(ProfileContext context)
        {
            int i = 0;
            var scrums = context.ScrumMasters.ToList();

            foreach (var scrum in scrums.Skip(scrums.Count / 2))
            {
                if (Random.Next(0, 10) < 5)
                {
                    i++;

                    scrum.User.UserName = $"ScrumMentor{i}";
                    _userManager.RemovePasswordAsync(scrum.Id).Wait();
                    _userManager.AddPasswordAsync(scrum.Id, $"Password{i}").Wait();
                    _userManager.AddToRoleAsync(scrum.Id, RoleType.Mentor).Wait();

                    var mentor = new Mentor
                    {
                        User = scrum.User,
                        SpecializationId = context.Specializations.First().Id
                    };

                    context.Mentors.Add(mentor);
                }
            }

            context.SaveChanges();
        }

        private void AddScrumGroup(ProfileContext context)
        {
            var groupCount = context.Groups.Count();
            int i = 1;

            foreach (var scrum in context.ScrumMasters.ToList())
            {
                scrum.CurrentGroup = (i < groupCount) ? context.Groups.FirstOrDefault(g => g.Id == i) : context.Groups.FirstOrDefault(g => g.Id == groupCount);
                scrum.Groups.Add(scrum.CurrentGroup);
                scrum.GroupId = (i < groupCount) ? i : groupCount;

                i++;
            }

            context.SaveChanges();

            var scrumMasters = context.ScrumMasters.Where(sm => sm.Groups.Count > 0).ToList();
            List<Trainee> trainees = context.Trainees.Where(t => t.GroupId.HasValue).ToList();

            var traineesPerSM = trainees.Count / scrumMasters.Count;

            for (i = 0; i < scrumMasters.Count; i++)
            {
                for (int j = i * traineesPerSM; j < (i + 1) * traineesPerSM && j < trainees.Count; j++)
                {
                    trainees[j].Group = scrumMasters[i].CurrentGroup;
                }
            }

            foreach (var trainee in trainees)
            {
                context.Entry(trainee).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        private void AddMentorSpecializationAndTrainees(ProfileContext context)
        {
            foreach (var mentor in context.Mentors.ToList())
            {
                mentor.SpecializationId = context.Specializations.First().Id;

                foreach (var specialization in context.Specializations)
                {
                    if (Random.Next(10) > 6)
                    {
                        mentor.SpecializationId = specialization.Id;
                        break;
                    }
                }
            }

            context.SaveChanges();

            int mentorCount = context.Mentors.Count();
            var trainees = context.Trainees.ToList();
            var mentors = context.Mentors.ToList();

            for (int i = 0; i < trainees.Count; i++)
            {
                trainees[i].MentorId = mentors[i % mentorCount].Id;
            }

            foreach (var trainee in trainees)
            {
                context.Entry(trainee).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        private void AddNewUsers(ProfileContext context)
        {
            var fullNameList = new List<string>
            {
                "Фарахов Александр", "Блиц Екатерина", "Ахтанов Степан",
                "Брезгун Константин", "Слепов Сергей", "Ахмедова Алина",
                "Лаков Дмитрий", "Васнецова Зарина", "Радионов Павел",
                "Серов Сергей", "Карпук Денис", "Зеленый Олег",
            };

            for (int i = 0; i < fullNameList.Count; i++)
            {
                var hr = new User
                {
                    Login = $"NewUserLogin{i}",
                    Email = $"NewUser{i}@mail.com",
                    FullName = $"{fullNameList[i]}"
                };

                hr.Contacts = new Contacts
                {
                    Email = $"NewUser{i}@mail.com"
                };

                _userManager.CreateAsync(hr, $"Password{i}").Wait();
            }
        }

        private void AddHRs(ProfileContext context)
        {
            int hrCount = 2;
            string[] companies = { "Synesis", "ITechArt" };

            for (int i = 1; i <= hrCount; i++)
            {
                var hr = new User
                {
                    Login = $"HRLogin{i}",
                    Email = $"hr{i}@mail.com",
                    FullName = $"HR{i}",

                    Contacts = new Contacts
                    {
                        Company = companies[i - 1],
                        Skype = $"hr{i}.skype"
                    }
                };

                _userManager.CreateAsync(hr, $"Password{i}").Wait();
                _userManager.AddToRoleAsync(hr.Id, RoleType.HR).Wait();
            }
        }

        private void AddAdmin(ProfileContext context)
        {
            var admin = new User
            {
                Login = "Admin",
                Email = "admin@mail.com",
                FullName = $"Админыч"
            };

            _userManager.CreateAsync(admin, $"Password").Wait();
            _userManager.AddToRoleAsync(admin.Id, RoleType.Admin).Wait();
        }

        private void AddPermissions(ProfileContext context)
        {
            var adminRole = context.Roles.Single(r => r.Type == RoleType.Admin);
            var hrRole = context.Roles.Single(r => r.Type == RoleType.HR);
            var managerRole = context.Roles.Single(r => r.Type == RoleType.Manager);
            var mentorRole = context.Roles.Single(r => r.Type == RoleType.Mentor);
            var smRole = context.Roles.Single(r => r.Type == RoleType.ScrumMaster);
            var traineeRole = context.Roles.Single(r => r.Type == RoleType.Trainee);

            adminRole.Permissions.Add(new Permission(PermissionType.OnlyAdmin));
            hrRole.Permissions.Add(new Permission(PermissionType.OnlyHR));
            managerRole.Permissions.Add(new Permission(PermissionType.OnlyManager));
            mentorRole.Permissions.Add(new Permission(PermissionType.OnlyMentor));
            smRole.Permissions.Add(new Permission(PermissionType.OnlyScrumMaster));
            traineeRole.Permissions.Add(new Permission(PermissionType.OnlyTrainee));

            context.SaveChanges();
        }

        private void AddRoles(ProfileContext context)
        {
            _roleManager.CreateAsync(new Role(RoleType.Admin)).Wait();
            _roleManager.CreateAsync(new Role(RoleType.Manager)).Wait();
            _roleManager.CreateAsync(new Role(RoleType.Trainee)).Wait();
            _roleManager.CreateAsync(new Role(RoleType.Mentor)).Wait();
            _roleManager.CreateAsync(new Role(RoleType.ScrumMaster)).Wait();
            _roleManager.CreateAsync(new Role(RoleType.HR)).Wait();
        }

        private void RemoveReviews(ProfileContext context)
        {
            int traineeWithoutReviewsCount = (int)(context.Trainees.Count() * 0.2);
            var trainees = context.Trainees
                .Take(traineeWithoutReviewsCount)
                .ToList();

            foreach (var trainee in trainees)
            {
                context.Reviews.RemoveRange(
                    context.Reviews.Where(
                        r => r.ReviewedTraineeId == trainee.Id));
            }

            context.SaveChanges();
        }

        private void AddManager(ProfileContext context)
        {
            var manager = new User
            {
                Login = "Manager",
                Email = "email_dovnar@mail.com",
                FullName = $"Довнар Алексей"
            };

            manager.Contacts = new Contacts
            {
                Email = "email_dovnar@mail.com",
                Skype = $"dovnar7",
                Phone = $"8029-512-64-33",
                LinkedIn = "LinkedIn"
            };

            _userManager.CreateAsync(manager, $"Password").Wait();
            _userManager.AddToRoleAsync(manager.Id, RoleType.Manager).Wait();
        }

        private void AddTeamReviews(ProfileContext context)
        {
            var trainees = context
                .Trainees.Where(t => t.GroupId.HasValue)
                .OrderBy(t => t.Id)
                .ToList().Take(25);

            foreach (var trainee in trainees)
            {
                var teammates = trainees
                    .Where(t => t.GroupId == trainee.GroupId && t.Id != trainee.Id);

                foreach (var teammate in teammates)
                {
                    if (Random.Next(0, 2) == 1)
                    {
                        var review = new Review
                        {
                            ReviewType = ReviewType.TraineeReview,
                            ReviewedTraineeId = trainee.Id,
                            ReviewerId = teammate.Id
                        };

                        context.Reviews.Add(review);

                        var gradedAbilities = context.Skills
                            .Where(a => a.SkillType == SkillType.Ability);

                        foreach (var ability in gradedAbilities)
                        {
                            var grade = new Grade
                            {
                                SkillId = ability.Id,
                                Comment = $"Comment from {teammate.User.FullName} about {ability.Name}",
                                Mark = Random.Next(1, 6),
                                Review = review
                            };

                            context.Grades.Add(grade);
                        }

                        var nonGradedAbilities = context.Skills
                            .Where(a => a.SkillType == SkillType.Strenghts ||
                                a.SkillType == SkillType.Weaknesses);

                        foreach (var ability in nonGradedAbilities)
                        {
                            var grade = new Grade
                            {
                                SkillId = ability.Id,
                                Comment = $"Comment from {teammate.User.FullName} about {ability.Name}",
                                Review = review
                            };

                            context.Grades.Add(grade);
                        }
                    }
                }
            }

            context.SaveChanges();
        }

        private void AddScrumMasters(ProfileContext context)
        {
            int smCount = context.Groups.Count(g => g.Project.Status != ProjectStatus.Created);

            var scrumNames = new List<string>
            {
                "Фролов Федр",
                "Хрусталев Дмитрий",
                "Баканов Алексей",
                "Семенов Алексей",
                "Грядов Петр"
            };

            for (int i = 1; i <= smCount; i++)
            {
                var user = new User
                {
                    Login = $"ScrumLogin{i}",
                    Email = $"email_{i}@mail.com",
                    FullName = i <= scrumNames.Count ?
                        scrumNames[i - 1] :
                        "Scrum FullName"
                };

                user.Contacts = new Contacts
                {
                    Email = $"email_{i}@gmail.com",
                    Skype = $"Skype-{i}",
                    Phone = $"000-{i}",
                    LinkedIn = $"LinkedIn-{i}"
                };

                _userManager.CreateAsync(user, $"Password{i}").Wait();

                var scrum = new ScrumMaster { User = user };

                context.ScrumMasters.Add(scrum);
            }

            context.SaveChanges();

            foreach (var scrum in context.ScrumMasters.ToList())
            {
                _userManager.AddToRoleAsync(scrum.Id, RoleType.ScrumMaster).Wait();
            }
        }

        private void AddEmptyScrums(ProfileContext context)
        {
            var scrumNames = new List<string> { "Будко Алена", "Даратон Сергей" };
            for (int i = 1; i <= scrumNames.Count; i++)
            {
                var user = new User
                {
                    Login = $"ScrumEmptyLogin{i}",
                    Email = $"email_{i}@mail.com",
                    FullName = scrumNames[i - 1]
                };

                user.Contacts = new Contacts
                {
                    Email = $"email_{i}@gmail.com",
                    Skype = $"Skype-{i}",
                    Phone = $"000-{i}",
                    LinkedIn = $"LinkedIn-{i}"
                };

                _userManager.CreateAsync(user, $"PasswordEmpty{i}").Wait();

                var scrum = new ScrumMaster { User = user };

                context.ScrumMasters.Add(scrum);
            }

            context.SaveChanges();

            foreach (var scrum in context.ScrumMasters.ToList())
            {
                _userManager.AddToRoleAsync(scrum.Id, RoleType.ScrumMaster).Wait();
            }
        }

        private void AddScrumReviews(ProfileContext context)
        {
            var scrumMasters = context.ScrumMasters.Where(sm => sm.Groups.Count > 0).ToList();
            List<Trainee> trainees = context.Trainees.Where(t => t.GroupId.HasValue).ToList();

            foreach (var sm in scrumMasters)
            {
                var scrumTrainees = sm.CurrentGroup.Trainees.Take(Random.Next(1, 3)).ToList();

                foreach (var trainee in scrumTrainees)
                {
                    if (trainee != null)
                    {
                        var review = new Review
                        {
                            ReviewerId = sm.Id,
                            ReviewedTraineeId = trainee.Id,
                            ReviewType = ReviewType.ScrumReview
                        };

                        review = context.Reviews.Add(review);

                        var grade = new Grade
                        {
                            Review = review,
                            Comment = $"Comment for {trainee.User.Login}",
                            Mark = Random.Next(1, 5)
                        };

                        context.Grades.Add(grade);
                    }
                }
            }

            context.SaveChanges();
        }

        private void AddProjects(ProfileContext context)
        {
            int projectsCount = 7;
            var projectNames = new List<string> { "Profile2", "Menu", "Pentagon", "WarHammer", "Dota3", "PVT", "Facebook" };
            for (int i = 1; i <= projectsCount; i++)
            {
                int wordCount = Random.Next(2, 12);
                var shortDescription = new StringBuilder(wordCount);

                for (int j = 0; j < wordCount; j++)
                {
                    shortDescription.Append("description ");
                }

                shortDescription.AppendLine();
                var randomMonths = Random.Next(1, 10);
                var randomYears = Random.Next(1, 4);
                context.Projects.Add(new Project
                {
                    Name = projectNames[i - 1],
                    FullDescription = $"Project-{i}-FullDescription",
                    ShortDescription = shortDescription.ToString(),
                    StartDate = new DateTime(2015, 1, 1).AddMonths(randomMonths).AddDays(Random.Next(1, 24)),
                    FinishDate = new DateTime(2015, 1, 1).AddYears(randomYears).AddDays(Random.Next(1, 24))
                });
            }

            context.SaveChanges();

            var waitingProjects = context.Projects.Where(p => p.Id < 3);

            foreach (var project in waitingProjects)
            {
                project.ShortDescription = null;
                project.FullDescription = null;
            }

            var archivedProject = context.Projects.ToList().Last();

            archivedProject.IsInArchive = true;

            context.SaveChanges();
        }

        // private void AddReleases(ProfileContext context)
        // {
        //    foreach (var project in context.Projects)
        //    {
        //        int releasesCount = Random.Next(1, 3);
        //
        //        for (int i = 1; i <= releasesCount; i++)
        //        {
        //            context.Releases.Add(new Release
        //            {
        //                Name = $"{project.Name}-Release-{i}",
        //                StartDate = DateTime.Now.AddDays(Random.Next(1, 30)),
        //                ReleaseDate = DateTime.Now.AddMonths(Random.Next(3, 6)),
        //                ProjectId = project.Id
        //            });
        //        }
        //    }
        //
        //    context.SaveChanges();
        // }
        private void AddGroups(ProfileContext context)
        {
            foreach (var project in context.Projects)
            {
                int groupsCount = Random.Next(2, 5);

                for (int i = 1; i <= groupsCount; i++)
                {
                    context.Groups.Add(new Group
                    {
                        Number = $"{project.Id}.{i}",
                        ProjectId = project.Id,
                        StartDate = new DateTime(2016, 9, 1),
                        FinishDate = new DateTime(2016, 12, 31),
                        TeamPurpose = "Purpose of the team",
                        TeamworkDescription = "Description of teamwork"
                    });
                }
            }

            context.SaveChanges();
        }

        private void AddSpecializations(ProfileContext context)
        {
            var specializations = new List<Specialization>
            {
                new Specialization { Name = "Front-end", Abbreviation = Specialization.Frontend },
                new Specialization { Name = "Back-end .NET", Abbreviation = Specialization.BackendNET },
                new Specialization { Name = "Back-end Android", Abbreviation = Specialization.BackendAndroid },
                new Specialization { Name = "Back-end Java ", Abbreviation = Specialization.BackendJava },
                new Specialization { Name = "Back-end PHP ", Abbreviation = Specialization.BackendPHP },
                new Specialization { Name = "Business analyst", Abbreviation = Specialization.BA },
                new Specialization { Name = "QA Engineer", Abbreviation = Specialization.QA },
                new Specialization { Name = "UI/UX Specialist", Abbreviation = Specialization.Design }
            };

            context.Specializations.AddRange(specializations);

            context.SaveChanges();
        }

        private void AddMainSkills(ProfileContext context)
        {
            int skillsCount = 5;

            foreach (var specialization in context.Specializations)
            {
                for (int i = 1; i <= skillsCount; i++)
                {
                    context.MainSkills.Add(new MainSkill
                    {
                        Name = $"Skill-{i}-Spec-{specialization.Id}",
                        Specialization = specialization
                    });
                }
            }

            context.SaveChanges();
        }

        private void AddTraineeProfiles(ProfileContext context)
        {
            foreach (var trainee in context.Trainees)
            {
                context.TraineeProfiles.Add(new TraineeProfile
                {
                    DesirablePosition = $"DesirablePosition-{trainee.Id}",
                    ExperienceAtITLab = $"CurrentPosition-{trainee.Id}",
                    ProfessionalPurposes = $"ProfessionalPurposes-{trainee.Id}",
                    CurrentWork = $"CurrentWork-{trainee.Id}",
                    CurrentPosition = $"000-{trainee.Id}",
                    JobDuties = $"000-{trainee.Id}",
                    EmploymentDuration = $"000-{trainee.Id}",
                    Strengths = $"000-{trainee.Id}",
                    Weaknesses = $"000-{trainee.Id}",
                    TraineeId = trainee.Id,
                    Trainee = trainee
                });
            }

            context.SaveChanges();
        }

        private void AddLanguages(ProfileContext context)
        {
            foreach (var traineeProfile in context.TraineeProfiles)
            {
                int languagesCount = Random.Next(4);

                for (int i = 0; i < languagesCount; i++)
                {
                    context.Languages.Add(new Language
                    {
                        Name = $"Name-{i}-Profile-{traineeProfile.Id}",
                        Level = $"Level-{i}-Profile-{traineeProfile.Id}",
                        TraineeProfileId = traineeProfile.Id
                    });
                }
            }

            context.SaveChanges();
        }

        private void AddUniversities(ProfileContext context)
        {
            foreach (var traineeProfile in context.TraineeProfiles)
            {
                int universitiesCount = Random.Next(4);

                for (int i = 0; i < universitiesCount; i++)
                {
                    context.Universities.Add(new University
                    {
                        AdditionalInformation = $"AdditionalInformation-{i}-Profile-{traineeProfile.Id}",
                        EducationalInstitution = $"EducationalInstitution-{i}-Profile-{traineeProfile.Id}",
                        TrainingPeriod = $"TrainingPeriod-{i}-Profile-{traineeProfile.Id}",
                        Specialization = $"Specialization-{i}-Profile-{traineeProfile.Id}",
                        Specialty = $"Specialty-{i}-Profile-{traineeProfile.Id}",
                        TraineeProfileId = traineeProfile.Id
                    });
                }
            }

            context.SaveChanges();
        }

        private void AddCourses(ProfileContext context)
        {
            foreach (var traineeProfile in context.TraineeProfiles)
            {
                int coursesCount = Random.Next(4);

                for (int i = 0; i < coursesCount; i++)
                {
                    context.Courses.Add(new Course
                    {
                        AdditionalInformation = $"AdditionalInformation-{i}-Profile-{traineeProfile.Id}",
                        EducationalEstablishment = $"EducationalEstablishment-{i}-Profile-{traineeProfile.Id}",
                        TimePeriod = $"TimePeriod-{i}-Profile-{traineeProfile.Id}",
                        CourseName = $"CourseName-{i}-Profile-{traineeProfile.Id}",
                        TraineeProfileId = traineeProfile.Id
                    });
                }
            }

            context.SaveChanges();
        }

        private void AddSoftSkills(ProfileContext context)
        {
            foreach (var traineeProfile in context.TraineeProfiles)
            {
                if (traineeProfile.Trainee.MentorId.HasValue)
                {
                    int softSkillsCount = traineeProfile.Trainee.MentorId.Value - 1;

                    for (int i = 0; i < softSkillsCount; i++)
                    {
                        context.SoftSkills.Add(new SoftSkill
                        {
                            Name = $"SoftSkills-{i}-Profile-{traineeProfile.Id}",
                            TraineeProfileId = traineeProfile.Id
                        });
                    }
                }
            }

            context.SaveChanges();
        }

        private void AddCurrentProjectData(ProfileContext context)
        {
            Project project = new Project
            {
                Name = "Profile",
                FullDescription = "FullDescription for project Profile",
                ShortDescription = "ShortDescription  for project Profile",
                StartDate = new DateTime(2016, 01, 19),
                FinishDate = new DateTime(2017, 08, 15),
            };
            context.Projects.Add(project);
            context.SaveChanges();

            Group group = new Group
            {
                Number = "PROF-Team 1",
                StartDate = new DateTime(2016, 9, 1),
                FinishDate = new DateTime(2016, 12, 31),
                ProjectId = project.Id,
                TeamPurpose = "Purpose of the team",
                TeamworkDescription = "Description of teamwork"
            };
            context.Groups.Add(group);
            context.SaveChanges();

            var specializations = context.Specializations.ToList();

            List<MainSkill> mainSkills = new List<MainSkill>
            {
                new MainSkill { Name = "Interaction design", Specialization = specializations[0] },
                new MainSkill { Name = "Mobile & tablet design", Specialization = specializations[0] },
                new MainSkill { Name = "Responsive design", Specialization = specializations[0] },
                new MainSkill { Name = "User centred design", Specialization = specializations[0] },
                new MainSkill { Name = "Wireframing & Prototyping", Specialization = specializations[0] },

                new MainSkill { Name = "Functional testing", Specialization = specializations[1] },
                new MainSkill { Name = "Requirements Analysis/Testing", Specialization = specializations[1] },
                new MainSkill { Name = "Test cases Development", Specialization = specializations[1] },
                new MainSkill { Name = "Test strategy development", Specialization = specializations[1] },
                new MainSkill { Name = "Writing Test Plan", Specialization = specializations[1] },
                new MainSkill { Name = "Defects Hunting", Specialization = specializations[1] },
                new MainSkill { Name = "Test Results Analysis and Reporting", Specialization = specializations[1] },
                new MainSkill { Name = "Software Development Models and Methodologies", Specialization = specializations[1] },

                new MainSkill { Name = "Software Development Models and Methodologies", Specialization = specializations[2] },
                new MainSkill { Name = "Architecture knowledge", Specialization = specializations[2] },
                new MainSkill { Name = ".NET Development", Specialization = specializations[2] },
                new MainSkill { Name = ".NET Web Development", Specialization = specializations[2] },
                new MainSkill { Name = "ASP.NET MVC Development", Specialization = specializations[2] },
                new MainSkill { Name = ".NET Back-End Development", Specialization = specializations[2] },
                new MainSkill { Name = ".NET RESTful services and WebAPI", Specialization = specializations[2] },
                new MainSkill { Name = ".NET Data Access Development", Specialization = specializations[2] },
                new MainSkill { Name = "SQL fundamentals", Specialization = specializations[2] },
                new MainSkill { Name = "HTML,CSS", Specialization = specializations[2] },

                new MainSkill { Name = "Software Development Models and Methodologies", Specialization = specializations[3] },
                new MainSkill { Name = "HTML,CSS", Specialization = specializations[3] },
                new MainSkill { Name = "jQuery", Specialization = specializations[3] },
                new MainSkill { Name = "Frameworks", Specialization = specializations[3] },
                new MainSkill { Name = "Git", Specialization = specializations[3] },
                new MainSkill { Name = "AJAX, JSON", Specialization = specializations[3] },
                new MainSkill { Name = "Adaptive Slicing", Specialization = specializations[3] },
                new MainSkill { Name = "REST", Specialization = specializations[3] },

                new MainSkill { Name = "Software Development Models and Methodologies", Specialization = specializations[4] },
                new MainSkill { Name = "Elicitation techniques", Specialization = specializations[4] },
                new MainSkill { Name = "Analyzing Requirements", Specialization = specializations[4] },
                new MainSkill { Name = "Documenting Requirements", Specialization = specializations[4] },
                new MainSkill { Name = "Prioritizing Requirements", Specialization = specializations[4] },
                new MainSkill { Name = "UML modeling", Specialization = specializations[4] },
                new MainSkill { Name = "BPMN modeling", Specialization = specializations[4] },
                new MainSkill { Name = "UI design for Business analysts", Specialization = specializations[4] },
                new MainSkill { Name = "Requirements management", Specialization = specializations[4] },
                new MainSkill { Name = "Modelling Tools ", Specialization = specializations[4] },
            };

            foreach (var mainSkill in mainSkills)
            {
                context.MainSkills.Add(mainSkill);
            }

            context.SaveChanges();

            List<Trainee> trainees = new List<Trainee>
            {
                new Trainee { User = new User { FullName = "Юзерович Юзер", Email = "UserovichUser@gmail.com" }, SpecializationId = specializations[0].Id, Group = group },
                new Trainee { User = new User { FullName = "Богойлов Николай", Email = "BaiholauMikalay@gmail.com" }, SpecializationId = specializations[0].Id, Group = group },
                new Trainee { User = new User { FullName = "Голынская Ольга", Email = "HalynskayaVolha@gmail.com" }, SpecializationId = specializations[1].Id, Group = group },
                new Trainee { User = new User { FullName = "Малаха Артур", Email = "MalahaArtur@gmail.com" }, SpecializationId = specializations[2].Id, Group = group },
                new Trainee { User = new User { FullName = "Муравицкий Максим", Email = "MuravickyMaksim@gmail.com" }, SpecializationId = specializations[0].Id, Group = group },
                new Trainee { User = new User { FullName = "Павлова Елена", Email = "PavlovaElena@gmail.com" }, SpecializationId = specializations[3].Id, Group = group },
                new Trainee { User = new User { FullName = "Павлюченко Валерий", Email = "PavluchenkoValeriy@gmail.com" }, SpecializationId = specializations[2].Id, Group = group },
                new Trainee { User = new User { FullName = "Петрова Алина", Email = "PetrovaAlina@gmail.com" }, SpecializationId = specializations[4].Id, Group = group },
                new Trainee { User = new User { FullName = "Романюк Дмитрий", Email = "RomaniukDmitry@gmail.com" }, SpecializationId = specializations[3].Id, Group = group },
                new Trainee { User = new User { FullName = "Семенов Алексей", Email = "SemenovAleksei@gmail.com", Login = "aleksey.semenoff" }, SpecializationId = specializations[4].Id, Group = group },
                new Trainee { User = new User { FullName = "Сергиевич Екатерина", Email = "SerhiyevichKatsiaryna@gmail.com" }, SpecializationId = specializations[1].Id, Group = group },
                new Trainee { User = new User { FullName = "Хилько Роман", Email = "surpriseinmailbox@gmail.com", Login = "surpriseinmailbox" }, SpecializationId = specializations[1].Id, Group = group },
                new Trainee { User = new User { FullName = "Кузнецов Артур", Email = "artorius01@gmail.com", Login = "artorius01" }, SpecializationId = specializations[4].Id, Group = group },
                new Trainee { User = new User { FullName = "Лемеш Вадим", Email = "freelinerr@gmail.com", Login = "freelinerr" }, SpecializationId = specializations[4].Id, Group = group },
                new Trainee { User = new User { FullName = "Вихарев Егор", Email = "shmurgee@mail.ru", Login = "shmurgee" }, SpecializationId = specializations[1].Id, Group = group },
                new Trainee { User = new User { FullName = "Романчик Ксения", Email = "ks.romanchik@gmail.com", Login = "ks.romanchik" }, SpecializationId = specializations[0].Id, Group = group },
                new Trainee { User = new User { FullName = "Корнеева Мария", Email = "skadiag41@gmail.com", Login = "skadiag41" }, SpecializationId = specializations[0].Id, Group = group },
                new Trainee { User = new User { FullName = "Сухов Владимир", Email = "excellentwebmaster@rambler.ru", Login = "excellentwebmaster" }, SpecializationId = specializations[1].Id, Group = group },
                new Trainee { User = new User { FullName = "Горгун Александр", Email = "chhwoo@tut.by", Login = "chhwoo" }, SpecializationId = specializations[7].Id, Group = group },
                new Trainee { User = new User { FullName = "Воронцов Артем", Email = "artem.vorontsof@gmail.com", Login = "fardvart" }, SpecializationId = specializations[6].Id, Group = group }, 
                //// Free trainees
                new Trainee { User = new User { FullName = "Бригерев Павел", Email = "UserovichUser@gmail.com" }, SpecializationId = specializations[0].Id },
                new Trainee { User = new User { FullName = "Среднев Алексей", Email = "BaiholauMikalay@gmail.com" }, SpecializationId = specializations[0].Id },
                new Trainee { User = new User { FullName = "Зарнова Ольга", Email = "HalynskayaVolha@gmail.com" }, SpecializationId = specializations[1].Id },
                new Trainee { User = new User { FullName = "Степнов Артур", Email = "MalahaArtur@gmail.com" }, SpecializationId = specializations[1].Id },
                new Trainee { User = new User { FullName = "Глазов Максим", Email = "MuravickyMaksim@gmail.com" }, SpecializationId = specializations[1].Id },
                new Trainee { User = new User { FullName = "Тран Елена", Email = "PavlovaElena@gmail.com" }, SpecializationId = specializations[0].Id },
                new Trainee { User = new User { FullName = "Щиев Валерий", Email = "PavluchenkoValeriy@gmail.com" }, SpecializationId = specializations[2].Id },
                new Trainee { User = new User { FullName = "Прокол Алина", Email = "PetrovaAlina@gmail.com" }, SpecializationId = specializations[2].Id },
                new Trainee { User = new User { FullName = "Степнов Дмитрий", Email = "RomaniukDmitry@gmail.com" }, SpecializationId = specializations[3].Id },
                new Trainee { User = new User { FullName = "Аранов Алексей", Email = "SemenovAleksei@gmail.com" }, SpecializationId = specializations[3].Id },
                new Trainee { User = new User { FullName = "Гренкина Екатерина", Email = "SerhiyevichKatsiaryna@gmail.com" }, SpecializationId = specializations[3].Id },
                new Trainee { User = new User { FullName = "Пробов Роман", Email = "surpriseinmailbox@gmail.com" }, SpecializationId = specializations[1].Id },
                new Trainee { User = new User { FullName = "Слонов Артур", Email = "artorius01@gmail.com" }, SpecializationId = specializations[2].Id },
                new Trainee { User = new User { FullName = "Клошев Вадим", Email = "freelinerr@gmail.com" }, SpecializationId = specializations[3].Id },
            };

            int fakeTraineeCount = 180;

            for (int i = 0; i < fakeTraineeCount; i++)
            {
                int specId = (i % specializations.Count) + 1;

                trainees.Add(new Trainee { User = new User { FullName = $"Юзер {i + 100}", Email = "UserovichUser@gmail.com" }, SpecializationId = specId, Group = group });
            }

            for (int i = 0; i < trainees.Count; i++)
            {
                var trainee = trainees[i];

                trainee.User.Contacts = new Contacts
                {
                    Email = $"Profile_{i}@mail.com",
                    Skype = $"Skype-{i}",
                    Phone = $"000-{i}",
                    LinkedIn = $"LinkedIn-{i}"
                };
            }

            for (int i = 0; i < trainees.Count; i++)
            {
                if (trainees[i].User.Login == null)
                {
                    trainees[i].User.Login = $"Login{i + 1}";
                }

                _userManager.CreateAsync(trainees[i].User, $"QwertY13").Wait();
                context.Trainees.Add(trainees[i]);
            }

            context.SaveChanges();

            foreach (var trainee in context.Trainees.ToList())
            {
                _userManager.AddToRoleAsync(trainee.Id, RoleType.Trainee).Wait();
            }
        }

        private void AddMentors(ProfileContext context)
        {
            int mentorsCount = 5;
            var mentorNames = new List<string>
            {
                "Скиба Алексей",
                "Грут Яесть",
                "Башко Павел",
                "Дмитрович Екатерина",
                "Лазанов Семен"
            };

            for (int i = 1; i <= mentorsCount; i++)
            {
                var user = new User
                {
                    Login = $"MentorLogin{i}",
                    Email = $"email_{i}@mail.com",
                    FullName = mentorNames[i - 1]
                };

                user.Contacts = new Contacts
                {
                    Email = $"email_{i}@gmail.com",
                    Skype = $"Skype-{i}",
                    Phone = $"000-{i}",
                    LinkedIn = $"LinkedIn-{i}"
                };

                _userManager.CreateAsync(user, $"Password{i}").Wait();

                var mentor = new Mentor
                {
                    User = user,
                    SpecializationId = context.Specializations.First().Id
                };

                context.Mentors.Add(mentor);
            }

            context.SaveChanges();

            foreach (var mentor in context.Mentors.ToList())
            {
                _userManager.AddToRoleAsync(mentor.Id, RoleType.Mentor).Wait();
            }
        }

        private void AddMentorReviews(ProfileContext context)
        {
            var mentors = context.Mentors.ToList();

            foreach (var m in mentors)
            {
                int count = m.Trainees.Count();
                List<Trainee> trainees = null;

                if (count != 0)
                {
                    trainees = m.Trainees.Skip(count / 2).ToList();
                }

                foreach (var t in trainees)
                {
                    var review = new Review
                    {
                        ReviewerId = m.Id,
                        ReviewedTraineeId = t.Id,
                        ReviewType = ReviewType.MentorReview
                    };

                    review = context.Reviews.Add(review);

                    var grade = new Grade
                    {
                        Review = review,
                        Comment = $"WorkComment for {t.User.Login}"
                    };

                    context.Grades.Add(grade);

                    context.SaveChanges();
                }
            }

            var reviews = context.Reviews
                .Where(r => r.ReviewType == ReviewType.MentorReview)
                .ToList();

            foreach (var r in reviews)
            {
                var grades = new List<Grade>();
                var mainSkills = r.ReviewedTrainee.Specialization.MainSkills;

                foreach (var s in mainSkills)
                {
                    grades.Add(new Grade
                    {
                        SkillId = s.Id,
                        ReviewId = r.Id,
                        Mark = Random.Next(1, 5),
                        Comment = $"Comment for mainskill {s.Name}"
                    });
                }

                var softSkills = r.ReviewedTrainee.TraineeProfile.SoftSkills;

                foreach (var s in softSkills)
                {
                    grades.Add(new Grade
                    {
                        SkillId = s.Id,
                        ReviewId = r.Id,
                        Mark = Random.Next(1, 5),
                        Comment = $"Comment for softskill {s.Name}"
                    });
                }

                context.Grades.AddRange(grades);
                context.SaveChanges();
            }
        }

        private void AddAbilities(ProfileContext context)
        {
            context.Skills.Add(new Skill
            {
                SkillType = SkillType.Ability,
                Name = "Навыки",
                Description = "Оцените профессиональные навыки коллеги в рамках его роли"
            });

            context.Skills.Add(new Skill
            {
                SkillType = SkillType.Ability,
                Name = "Коммуникация",
                Description = "Оцените навыки общения в рамках рабочего процесса"
            });

            context.Skills.Add(new Skill
            {
                SkillType = SkillType.Ability,
                Name = "Тайм-менеджмент",
                Description = "Оцените умение грамотно распределять время"
            });

            context.Skills.Add(new Skill
            {
                SkillType = SkillType.Ability,
                Name = "Работа в команде",
                Description = "Оцените работу в команде в рамках рабочего процесса"
            });

            context.Skills.Add(new Skill
            {
                SkillType = SkillType.Ability,
                Name = "Стрессоустойчивость",
                Description = "Оцените умение держаться в стрессовых ситуациях"
            });

            context.Skills.Add(new Skill
            {
                SkillType = SkillType.Ability,
                Name = "Вклад",
                Description = "Оцените вклад в рабочий процесс"
            });

            context.Skills.Add(new Skill
            {
                SkillType = SkillType.Ability,
                Name = "Креативность",
                Description = "Оцените творческий подход, умение предложить нестандартное решение"
            });

            context.Skills.Add(new Skill
            {
                SkillType = SkillType.Ability,
                Name = "Приоритеты",
                Description = "Оцените умение правильно расставлять приоритеты"
            });

            context.Skills.Add(new Skill
            {
                SkillType = SkillType.Weaknesses,
                Name = "Слабые стороны",
                Description = "Назовите слабые стороны коллеги, на что следует обратить внимание"
            });

            context.Skills.Add(new Skill
            {
                SkillType = SkillType.Strenghts,
                Name = "Сильные стороны",
                Description = "Назовите сильные стороны коллеги, за что благодарны в рамках рабочего процесса"
            });

            context.SaveChanges();
        }
    }
}
