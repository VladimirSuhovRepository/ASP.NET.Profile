using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;
using Profile.UI.Controllers;
using Profile.UI.ModelEnums;
using Profile.UI.Models.Manager;

namespace Profile.UI.Tests.Controller
{
    [TestClass]
    public class ManagerControllerTest : TransactionalTest
    {
        private ManagerController _controller;
        private IProfileContext _context;

        [TestInitialize]
        public void Setup()
        {
            NewScope();
            _controller = Kernel.Get<ManagerController>();
            NewScope();
            _context = Kernel.Get<IProfileContext>();
        }

        [TestMethod]
        public void ManageProjects_View_NotNull()
        {
            var result = _controller.ManageProjects() as ViewResult;

            Assert.IsNotNull(result, "View is null");
        }

        [TestMethod]
        public void ManageProjects_ViewModel_HasAllProjects()
        {
            // Actual data
            var result = _controller.ManageProjects() as ViewResult;
            var model = result.Model as ManagerProjectsMenuViewModel;

            // Expected data
            var projectsCount = _context.Projects.Where(p => !p.IsInArchive).Count();

            Assert.IsNotNull(model, "View model is empty for list of projects");
            Assert.AreEqual(projectsCount, 
                            model.Projects.Count,
                            $"Model have {model.Projects.Count} projects but {projectsCount} expected");
        }

        [TestMethod]
        public void ManagerGetTeam_GetJsonTeamModel_NotNull()
        {
            var someGroup = GetFirstGroup();

            var result = _controller.GetTeam(someGroup.Id) as JsonResult;

            Assert.IsNotNull(result, $"jsonResult is null for group with id: {someGroup.Id}");
            Assert.IsNotNull(result.Data, $"json data is null for group with id: {someGroup.Id}");
        }

        [TestMethod]
        public void ManagerGetTeam_GetJsonTeamModel_EqualToInputGroup()
        {
            var someGroup = GetFirstGroup();

            var result = _controller.GetTeam(someGroup.Id) as JsonResult;

            CompareData(someGroup, result.Data as ManagerTeamJsonResponseModel);
        }

        [TestMethod]
        public void ManagerDeleteProject_SetProjectIsInArchive_CorrectProperties()
        {
            var project = _context.Projects.FirstOrDefault(p => !p.IsInArchive && p.Groups.Count > 0);

            if (project == null)
            {
                Assert.Fail("No found suitable project for this test");
            }

            var team = project.Groups.FirstOrDefault();

            // Act
            var result = _controller.Delete(team.Id) as PartialViewResult;
            var model = result.Model as ManagerProjectsMenuViewModel;
            var deletedProject = _context.Projects.AsNoTracking().Single(p => p.Id == project.Id);

            // Expected
            var projectsCount = _context.Projects.AsNoTracking().Where(p => !p.IsInArchive && p.Status != ProjectStatus.InArchive).Count();
            var isInArchive = deletedProject.IsInArchive;
            var hasArchiveStatus = deletedProject.Status == ProjectStatus.InArchive;

            Assert.IsNotNull(model, "Model is null");
            Assert.AreEqual(projectsCount, 
                            model.Projects.Count,
                            $"Model have {model.Projects.Count} count of projects but {projectsCount} expected");
            Assert.IsTrue(isInArchive, "isInArchive of deleted project doesn't set to true");
            Assert.IsTrue(hasArchiveStatus, "Project has invalid status");
        }

        [TestMethod]
        public void ManagerAddProject_CreateForm_CorrectModelAndPartial()
        {
            // Actual data
            var result = _controller.AddNewProject() as PartialViewResult;
            var model = result.Model as TeamProjectInputViewModel;

            // Expected data
            var scrumMastersCount = _context.ScrumMasters.Count();
            var specializationsCount = _context.Specializations.Count();

            Assert.IsNotNull(result, "View is null");
            Assert.IsNotNull(result.Model, "Model is null");
            Assert.AreEqual(scrumMastersCount,
                            model.ScrumMasters.Count,
                            $"Model have {model.ScrumMasters.Count} scrum-masters but {scrumMastersCount} expected");
            Assert.AreEqual(specializationsCount,
                            model.Specializations.Count,
                            $"Model have {model.Specializations.Count} specializations but {specializationsCount} expected");
        }

        [TestMethod]
        public void ManagerAddProject_AddProjectToDatabase_CorrectData()
        {
            // Input data
            var freeTraineesIds = _context.Trainees.Where(t => !t.GroupId.HasValue).Select(t => t.Id).ToList();

            if (freeTraineesIds.Count < 3)
            {
                Assert.Fail("Too little count of trainees without group for this test");
            }

            var mentorId = _context.Mentors.FirstOrDefault().Id;
            #region fakeJsonProject
            var fakeJsonProject = new TeamProjectPostJsonModel
            {
                Name = "TestAddingProject",
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddMonths(5),
                Team = new TeamPostJsonModel
                {
                    Number = "TestAddingTeam-1",
                    StartDate = DateTime.Now,
                    FinishDate = DateTime.Now.AddDays(20),
                    ScrumId = _context.ScrumMasters.FirstOrDefault().Id,
                    Trainees = new List<ManagerTraineeJsonModel>
                    {
                        new ManagerTraineeJsonModel { Id = freeTraineesIds[0], MentorId = mentorId },
                        new ManagerTraineeJsonModel { Id = freeTraineesIds[1], MentorId = mentorId },
                        new ManagerTraineeJsonModel { Id = freeTraineesIds[2], MentorId = mentorId }
                    }
                }
            };
            #endregion

            // Act
            var result = _controller.AddNewProject(fakeJsonProject) as JsonResult;
            var actualTraineeCountInNewTeam = _context.Groups
                                                      .FirstOrDefault(g => (g.Number == fakeJsonProject.Team.Number) &&
                                                       (g.Id == (int)result.Data))
                                                      .Trainees.Count();

            // Expected
            var traineeCount = fakeJsonProject.Team.Trainees.Count;
            var addingProject = _context.Projects.SingleOrDefault(p => p.Name == fakeJsonProject.Name);
            var addingTeam = _context.Groups.FirstOrDefault(g => (g.Number == fakeJsonProject.Team.Number) &&
                                                                 (g.Id == (int)result.Data));

            // Assert
            Assert.IsNotNull(result.Data, "Json is null");
            Assert.IsTrue(addingProject != null, "New project not found in database");
            Assert.IsTrue(addingTeam != null, "New Team not found in database");
            Assert.AreEqual(traineeCount,
                            actualTraineeCountInNewTeam,
                            $"New Team has {actualTraineeCountInNewTeam} but {traineeCount} expected");
            Assert.IsNotNull(result.Data, "Result data is empty");
            Assert.AreEqual(result.Data,
                          addingTeam.Id,
                          $"Returnable id of team in result is {result.Data}, but {addingTeam.Id} expected");
        }

        [TestMethod]
        public void ManagerAddTeam_CreateForm_CorrectModelAndPartial()
        {
            // Actual data
            var projectId = _context.Projects.FirstOrDefault().Id;
            var result = _controller.AddTeam(projectId) as PartialViewResult;
            var model = result.Model as TeamProjectInputViewModel;

            // Expected data
            var project = _context.Projects.Find(projectId);
            var scrumMastersCount = _context.ScrumMasters.Count();
            var specializationsCount = _context.Specializations.Count();

            Assert.IsNotNull(result, "View is null");
            Assert.IsNotNull(result.Model, "Model is null");
            Assert.AreEqual(scrumMastersCount,
                            model.ScrumMasters.Count,
                            $"Model have {model.ScrumMasters.Count} scrum-masters but {scrumMastersCount} expected");
            Assert.AreEqual(specializationsCount,
                            model.Specializations.Count,
                            $"Model have {model.Specializations.Count} specializations but {specializationsCount} expected");
            Assert.IsNotNull(model.Project, "Project can not be null");
            Assert.AreEqual(project.Name,
                           model.Project.Name,
                           $"Project have {model.Project.Name} name but {project.Name} expected");
        }

        [TestMethod]
        public void ManagerAddTeam_AddTeamToDatabase_CorrectData()
        {
            // Input data
            var freeTraineesIds = _context.Trainees.Where(t => !t.GroupId.HasValue).Select(t => t.Id).ToList();
            var project = _context.Projects.AsNoTracking().FirstOrDefault();

            if (freeTraineesIds.Count < 3)
            {
                Assert.Fail("Too little count of trainees without group for this test");
            }

            var mentorId = _context.Mentors.FirstOrDefault().Id;
            #region fakeJsonTeam
            var fakeJsonProject = new TeamProjectPostJsonModel
            {
                ProjectId = project.Id,
                Team = new TeamPostJsonModel
                {
                    Number = "TestAddingTeam-1",
                    StartDate = DateTime.Now,
                    FinishDate = DateTime.Now.AddDays(20),
                    ScrumId = _context.ScrumMasters.FirstOrDefault().Id,
                    Trainees = new List<ManagerTraineeJsonModel>
                    {
                        new ManagerTraineeJsonModel { Id = freeTraineesIds[0], MentorId = mentorId },
                        new ManagerTraineeJsonModel { Id = freeTraineesIds[1], MentorId = mentorId },
                        new ManagerTraineeJsonModel { Id = freeTraineesIds[2], MentorId = mentorId }
                    }
                }
            };
            #endregion

            // Act
            var result = _controller.AddTeam(fakeJsonProject) as JsonResult;
            var actualTraineeCountInNewTeam = _context.Groups
                                                      .FirstOrDefault(g => (g.Number == fakeJsonProject.Team.Number) &&
                                                       (g.Id == (int)result.Data))
                                                      .Trainees.Count();

            // Expected
            var traineeCount = fakeJsonProject.Team.Trainees.Count;
            var projectOfTeam = _context.Projects.SingleOrDefault(p => p.Id == fakeJsonProject.ProjectId);
            var addingTeam = _context.Groups.FirstOrDefault(g => (g.Number == fakeJsonProject.Team.Number) &&
                                                                 (g.Id == (int)result.Data));

            // Assert
            Assert.IsNotNull(result.Data, "Json is null");
            Assert.IsTrue(addingTeam != null, "New Team not found in database");
            Assert.AreEqual(fakeJsonProject.ProjectId, 
                            addingTeam.Project.Id,
                            $"New Team has project id {addingTeam.Project.Id} but {fakeJsonProject.ProjectId} expected");
            Assert.AreEqual(traineeCount,
                            actualTraineeCountInNewTeam,
                            $"New Team has {actualTraineeCountInNewTeam} but {traineeCount} expected");
            Assert.IsNotNull(result.Data, "Result data is empty");
            Assert.AreEqual(result.Data,
                          addingTeam.Id,
                          $"Returnable id of team in result is {result.Data}, but {addingTeam.Id} expected");
        }

        [TestMethod]
        public void ManagerUpdateProject_UpdateProject_CorrectData()
        {
            // Input data
            var targetGroup = _context.Groups.Where(g => g.ProjectId.HasValue && 
                                                   g.Trainees.Count > 2).FirstOrDefault();
            var freeTrainee = _context.Trainees.Where(t => !t.GroupId.HasValue).FirstOrDefault();

            if (targetGroup == null)
            {
                Assert.Fail("Not found suitable group for this test");
            }

            if (freeTrainee == null)
            {
                Assert.Fail("Not found trainee without group in database for this test");
            }

            var mentorId = _context.Mentors.FirstOrDefault().Id;
            #region fakeJsonData
            var fakeTrainees = new List<ManagerTraineeJsonModel>();

            for (int i = 0; i < targetGroup.Trainees.Count - 1; i++)
            {
                var trainee = targetGroup.Trainees.ElementAt(i);

                fakeTrainees.Add(new ManagerTraineeJsonModel
                {
                    Id = trainee.Id,
                    MentorId = mentorId
                });
            }

            fakeTrainees.Add(new ManagerTraineeJsonModel
            {
                Id = freeTrainee.Id,
                FullName = freeTrainee.User.FullName,
                MentorId = mentorId
            });
            var fakeJsonData = new TeamProjectPostJsonModel
            {
                Name = targetGroup.Project.Name + "UpdatedName",
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddMonths(5),
                Team = new TeamPostJsonModel
                {
                    Id = targetGroup.Id,
                    Number = targetGroup.Number + "UpdatedNumber",
                    StartDate = DateTime.Now,
                    FinishDate = DateTime.Now.AddDays(20),
                    ScrumId = _context.ScrumMasters.FirstOrDefault().Id,
                    Trainees = fakeTrainees,
                }
            };
            #endregion
            var traineeKeys = fakeJsonData.Team.Trainees.Select(t => t.Id).ToArray();

            // Act
            var result = _controller.UpdateTeamProject(fakeJsonData) as JsonResult;
            var updatedProject = _context.Projects.AsNoTracking().SingleOrDefault(p => p.Name == fakeJsonData.Name &&
                                            p.Id == targetGroup.Project.Id);
            var updatedTeam = _context.Groups.AsNoTracking().FirstOrDefault(g => (g.Number == fakeJsonData.Team.Number) &&
                                                                       (g.Id == (int)result.Data));
            var updatedTraineesWithNewMentorIdCount = updatedTeam.Trainees.Where(t => traineeKeys.Contains(t.Id) &&
                                                                  t.MentorId == mentorId).Count();

            // Expected
            var traineeCount = fakeJsonData.Team.Trainees.Count;

            // Assert
            Assert.IsNotNull(result.Data, "Json is null");
            Assert.IsNotNull(updatedProject, "Updated project not found in database");
            Assert.AreEqual(fakeJsonData.StartDate.Date, 
                            updatedProject.StartDate.Date,
                            "Incorrect StartDate field for updated project");
            Assert.AreEqual(fakeJsonData.FinishDate.Date,
                            updatedProject.FinishDate.Date,
                            "Incorrect FinishDate field for updated project");
            Assert.IsNotNull(updatedTeam, "New Team not found in database");
            Assert.AreEqual(fakeJsonData.Team.StartDate.Date,
                           updatedTeam.StartDate.Date,
                           "Incorrect StartDate field for updated team");
            Assert.AreEqual(fakeJsonData.Team.FinishDate.Date,
                           updatedTeam.FinishDate.Date,
                           "Incorrect FinishDate field for updated team");
            Assert.AreEqual(fakeJsonData.Team.ScrumId,
                            updatedTeam.ScrumMasterId,
                            "Incorrect ScrumMasterId field for updated team");
            Assert.IsNotNull(updatedTeam.Trainees, "New team has no trainees");
            Assert.AreEqual(traineeCount, 
                            updatedTraineesWithNewMentorIdCount, 
                            "Expected count of trainees does not equal to actual count");
        }

        [TestMethod]
        public void ManagerDeleteUsers_DeleteUsersById()
        {
            var usersForRemoving = _context.Users.Where(u => u.Roles.Count() == 0)
                                                    .Take(3).Select(u => u.Id).ToArray();
            var fakeJson = new NewUsersForRemovingJsonModel
            {
                UserIds = usersForRemoving
            };

            // Act
            try
            {
                var result = _controller.DeleteUsers(fakeJson) as EmptyResult;
            }
            catch(Exception error)
            {
                Assert.Fail(error.Message);
            }
            // Expected
            var areUsersDeleted = !_context.Users.Any(u => fakeJson.UserIds.Contains(u.Id));

            Assert.IsTrue(areUsersDeleted, "Not all users removed from database");
        }

        [TestMethod]
        public void ManagerSetRoles_SetMentorRole_UserHasMentorRoleAndRecordInMentorTable()
        {
            var fakeJson = GetFakeJsonForSetRoles(3, UIRoleType.Mentor);
            var mentorRole = _context.Roles.Single(r => r.Type == RoleType.Mentor);

            // Act
            var result = _controller.SetRoles(fakeJson) as ContentResult;
            var newMentors = _context.Mentors.Where(m => fakeJson.UserIds.Contains(m.Id)).ToList();
            var actualUsers = _context.Users.Where(u => fakeJson.UserIds.Contains(u.Id)).ToList();
            var usersWithMentorRole = actualUsers.Where(u => u.Roles.Any(r => r.RoleId == mentorRole.Id)).ToList();

            // Expected
            var invalidSpecialization = newMentors.Any(m => m.SpecializationId != fakeJson.SpecializationId);

            Assert.AreEqual(fakeJson.UserIds.Length,
                            newMentors.Count,
                            $"Database has {newMentors.Count} of mentors but {fakeJson.UserIds.Length} expected");
            Assert.IsFalse(invalidSpecialization, "Updated users have invalid specialization");
            Assert.AreEqual(fakeJson.UserIds.Length,
                            usersWithMentorRole.Count,
                            $"Not all users has Mentor role, actual:{usersWithMentorRole.Count}, expected: {fakeJson.UserIds.Length}");
        }

        [TestMethod]
        public void ManagerSetRoles_SetScrumMasterRole_UserHasSMRoleAndRecordInSMTable()
        {
            var fakeJson = GetFakeJsonForSetRoles(3, UIRoleType.ScrumMaster);
            var scrumRole = _context.Roles.Single(r => r.Type == RoleType.ScrumMaster);

            // Act
            var result = _controller.SetRoles(fakeJson) as ContentResult;
            var newScrumMasters = _context.ScrumMasters.Where(sm => fakeJson.UserIds.Contains(sm.Id)).ToList();
            var actualUsers = _context.Users.Where(u => fakeJson.UserIds.Contains(u.Id)).ToList();
            var usersWithScrumMasterRole = actualUsers.Where(u => u.Roles.Any(r => r.RoleId == scrumRole.Id)).ToList();

            Assert.AreEqual(fakeJson.UserIds.Length,
                            newScrumMasters.Count,
                            $"Database has {newScrumMasters.Count} of scrumMasters but {fakeJson.UserIds.Length} expected");
            Assert.AreEqual(fakeJson.UserIds.Length,
                            usersWithScrumMasterRole.Count,
                            $"Not all users has ScrumMaster role, actual:{usersWithScrumMasterRole.Count}, expected: {fakeJson.UserIds.Length}");
        }

        [TestMethod]
        public void ManagerSetRoles_SetHRRole_UserHasHRRole()
        {
            var fakeJson = GetFakeJsonForSetRoles(3, UIRoleType.HR);
            var roleHR = _context.Roles.Single(r => r.Type == RoleType.HR);

            // Act
            var result = _controller.SetRoles(fakeJson) as ContentResult;
            var actualUsers = _context.Users.Where(u => fakeJson.UserIds.Contains(u.Id)).ToList();
            var usersWithHRRole = actualUsers.Where(u => u.Roles.Any(r => r.RoleId == roleHR.Id)).ToList();

            Assert.AreEqual(fakeJson.UserIds.Length,
                            usersWithHRRole.Count,
                            $"Not all users has HR role, actual:{usersWithHRRole.Count}, expected: {fakeJson.UserIds.Length}");
        }

        [TestMethod]
        public void ManagerSetRoles_SetTraineeRole_UserHasTraineeRoleAndRecordInTraineeTable()
        {
            var fakeJson = GetFakeJsonForSetRoles(3, UIRoleType.Trainee);
            var traineeRole = _context.Roles.Single(r => r.Type == RoleType.Trainee);

            // Act
            var result = _controller.SetRoles(fakeJson) as ContentResult;
            var newTrainees = _context.Trainees.Where(t => fakeJson.UserIds.Contains(t.Id) && t.TraineeProfile != null).ToList();
            var actualUsers = _context.Users.Where(u => fakeJson.UserIds.Contains(u.Id)).ToList();
            var usersWithTraineeRole = actualUsers.Where(u => u.Roles.Any(r => r.RoleId == traineeRole.Id)).ToList();

            // Expected
            var invalidSpecialization = newTrainees.Any(t => t.SpecializationId != fakeJson.SpecializationId);

            Assert.AreEqual(fakeJson.UserIds.Count(),
                            newTrainees.Count,
                            $"Database has {newTrainees.Count} of scrumMasters but {fakeJson.UserIds.Count()} expected");
            Assert.IsFalse(invalidSpecialization, "Updated users have invalid specialization");
            Assert.AreEqual(fakeJson.UserIds.Length,
                           usersWithTraineeRole.Count,
                           $"Not all users has Trainee role, actual:{usersWithTraineeRole.Count}, expected: {fakeJson.UserIds.Length}");
        }

        [TestMethod]
        public void ManagerSetRoles_SetMentorScrumMasterRole_UserHasTwoRolesAndRecordsInSmAndMentorTable()
        {
            var fakeJson = GetFakeJsonForSetRoles(3, UIRoleType.MentorScrumMaster);
            var scrumRole = _context.Roles.Single(r => r.Type == RoleType.ScrumMaster);
            var mentorRole = _context.Roles.Single(r => r.Type == RoleType.Mentor);

            // Act
            var result = _controller.SetRoles(fakeJson) as ContentResult;
            var newMentors = _context.Mentors.Where(m => fakeJson.UserIds.Contains(m.Id)).ToList();
            var newScrumMasters = _context.ScrumMasters.Where(sm => fakeJson.UserIds.Contains(sm.Id)).ToList();
            var actualUsers = _context.Users.Where(u => fakeJson.UserIds.Contains(u.Id)).ToList();
            var usersWithScrumRole = actualUsers.Where(u => u.Roles.Any(r => r.RoleId == scrumRole.Id)).ToList();
            var usersWithMentorRole = actualUsers.Where(u => u.Roles.Any(r => r.RoleId == mentorRole.Id)).ToList();

            // Expected
            var invalidSpecialization = newMentors.Any(t => t.SpecializationId != fakeJson.SpecializationId);

            Assert.AreEqual(fakeJson.UserIds.Length,
                            usersWithScrumRole.Count,
                            $"Not all users has ScrumMaster role, actual:{usersWithScrumRole.Count}, expected: {fakeJson.UserIds.Length}");
            Assert.AreEqual(fakeJson.UserIds.Length,
                            usersWithMentorRole.Count,
                            $"Not all users has Mentor role, actual:{usersWithMentorRole.Count}, expected: {fakeJson.UserIds.Length}");
            Assert.AreEqual(fakeJson.UserIds.Length,
                            newMentors.Count,
                            $"Database has {newMentors.Count} records in Mentor table for users  but {fakeJson.UserIds.Length} expected");
            Assert.AreEqual(fakeJson.UserIds.Length,
                            newScrumMasters.Count,
                            $"Database has {newScrumMasters.Count} records in ScrumMaster table for users  but {fakeJson.UserIds.Length} expected");
            Assert.IsFalse(invalidSpecialization, "Updated users have invalid specialization");
        }

        [TestMethod]
        public void ManagerUpdateProject_CreateForm_CorrectModelAndPartial()
        {
            // Actual data
            var team = _context.Groups.Where(g => g.ProjectId.HasValue && 
                                                  g.Trainees.Count > 2).FirstOrDefault();
            var result = _controller.UpdateTeamProject(team.Id) as PartialViewResult;
            var model = result.Model as TeamProjectInputViewModel;

            // Expected data
            var teamTraineesCount = team.Trainees.Count;
            var formTraineesCount = _context.Trainees.Where(t => !t.GroupId.HasValue).Count() + team.Trainees.Count;
            var mentorsCount = _context.Mentors.Count();
            var scrumMastersCount = _context.ScrumMasters.Count();
            var specializationsCount = _context.Specializations.Count();

            Assert.IsNotNull(result, "View is null");
            Assert.IsNotNull(result.Model, "Model is null");
            Assert.AreEqual(scrumMastersCount,
                            model.ScrumMasters.Count,
                            $"Model have {model.ScrumMasters.Count} scrum-masters but {scrumMastersCount} expected");
            Assert.AreEqual(specializationsCount,
                            model.Specializations.Count,
                            $"Model have {model.Specializations.Count} specializations but {specializationsCount} expected");
            Assert.IsNotNull(model.Project, "Project is null");
            Assert.IsNotNull(model.Group, "Team is null");
            Assert.IsNotNull(model.Group.Trainees, "Team has not trainees");
            Assert.AreEqual(teamTraineesCount,
                            model.Group.Trainees.Count,
                            $"Group has {model.Group.Trainees.Count} trainees but {teamTraineesCount} expected");
            Assert.AreEqual(mentorsCount,
                            model.MentorsForSelect.Count,
                            $"Model has {model.MentorsForSelect.Count} mentors but {mentorsCount} expected");
            Assert.AreEqual(formTraineesCount,
                            model.TraineesForSelect.Count,
                            $"Model has {model.MentorsForSelect.Count} mentors but {mentorsCount} expected");
        }

        [TestMethod]
        public void ManagerGetProjectsMenu_GettingPartial_CorrectPartialAndModel()
        {
            var group = _context.Groups.FirstOrDefault();

            var result = _controller.GetProjectsMenu(group.Id) as PartialViewResult;
            var model = result.Model as ManagerProjectsMenuViewModel;

            var projectsCount = _context.Projects.Where(p => !p.IsInArchive).Count();

            Assert.IsNotNull(result, "Partial is null");
            Assert.AreEqual(group.Id,
                            model.ActiveTeamId,
                            $"Actual new team id {model.ActiveTeamId} but {group.Id} expected");
            Assert.AreEqual(projectsCount,
                            model.Projects.Count,
                            $"Actual projects count is {model.Projects.Count} but {projectsCount} expected");
        }

        [TestMethod]
        public void ManageRoles_View_NotNull()
        {
            var userProvider = Kernel.Get<IUsersProvider>();

            // Act
            var result = _controller.ManageRoles() as ViewResult;
            var model = result.Model as NewUsersSetRolesViewModel;

            // expected data
            var specializationsCount = _context.Specializations.Count();
            var newUsersCount = userProvider.GetNewUsers().Count();

            Assert.IsNotNull(result, "View is null");
            Assert.IsNotNull(result.Model, "Model is null");
            Assert.AreEqual(newUsersCount,
                            model.Users.Count,
                            $"Model has {model.Users.Count} users but {newUsersCount} expected");
            Assert.AreEqual(specializationsCount,
                            model.Specializations.Count,
                            $"Model has {model.Specializations.Count} specializations but {specializationsCount} expected");
        }

        private NewUsersToRolesJsonModel GetFakeJsonForSetRoles(int userCount, UIRoleType role)
        {
            var newUsersIds = _context.Users.Where(u => u.Roles.Count() == 0).Take(userCount).Select(u => u.Id).ToArray();
            var newSpecialization = _context.Specializations.FirstOrDefault();
            return new NewUsersToRolesJsonModel
            {
                Role = role,
                SpecializationId = newSpecialization.Id,
                UserIds = newUsersIds
            };
        }

        private Group GetFirstGroup()
        {
            return _context.Groups.FirstOrDefault();
        }

        private void CompareData(Group inputModel, ManagerTeamJsonResponseModel outputModel)
        {
            Assert.AreEqual(inputModel.Project.Name, outputModel.ProjectName);

            if (inputModel.ScrumMaster != null)
            {
                Assert.AreEqual(inputModel.ScrumMaster.User.FullName, 
                                outputModel.ScrumName, 
                                "Different Scrum names");
            }

            Assert.AreEqual(inputModel.Number, outputModel.TeamName, "Different Team numbers");

            if ((inputModel != null) && (inputModel.Trainees.Count > 0))
            {
                var inputTrainees = inputModel.Trainees.ToList();
                Assert.AreEqual(inputTrainees.Count, 
                                outputModel.Trainees.Count, 
                                "Different count of trainees");

                for (int i = 0; i < inputTrainees.Count; i++)
                {
                    var outputTrainee = outputModel.Trainees.Single(t => t.Id == inputTrainees[i].Id);
                    Assert.IsNotNull(outputTrainee, $"trainee with id {inputTrainees[i].Id} not exist in model");          
                    Assert.AreEqual(inputTrainees[i].User.FullName,
                                    outputTrainee.FullName, 
                                    "Different trainee Name");

                    if (inputTrainees[i].Specialization != null)
                    {
                        Assert.AreEqual(inputTrainees[i].Specialization.Abbreviation,
                                       outputTrainee.Specialization, 
                                        "Different trainee Specialization");
                    }

                    if (inputTrainees[i].Mentor != null)
                    {
                        Assert.AreEqual(inputTrainees[i].Mentor.User.FullName,
                                        outputTrainee.MentorName, 
                                        "Different trainee Mentor name");
                    }
                }
            }
        }
    }
}
