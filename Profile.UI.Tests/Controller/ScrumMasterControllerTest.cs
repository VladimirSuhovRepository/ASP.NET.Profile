using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;
using Profile.UI.Controllers;
using Profile.UI.Mappers;
using Profile.UI.Models;
using Profile.UI.Tests.Infrastructure;
using Profile.UI.Tests.Ninject;

namespace Profile.UI.Tests.Controller
{
    [TestClass]
    public class ScrumMasterControllerTest : TransactionalTest
    {
        private IProfileContext _context;

        [TestInitialize]
        public void Setup()
        {
            NewScope();
            _context = Kernel.Get<IProfileContext>();
        }

        [TestMethod]
        public void ScrumMasterGetProjectAndModel_NotNull()
        {
            var scrumMasterId = _context.Groups.First(t => t.Project.Status == ProjectStatus.WaitingForDescription).ScrumMasterId.Value;
            var controller = CreateControllerWithFakeUser(scrumMasterId);
            var dbGroupInital = _context.ScrumMasters.Find(scrumMasterId).CurrentGroup;

            ViewResult result = controller.Project() as ViewResult;
            Assert.IsNotNull(result, "View is null");
            ScrumProjectViewModel model = result.ViewData.Model as ScrumProjectViewModel;
            Assert.IsNotNull(model, "Model is null");
            var dbGroup = _context.ScrumMasters.Find(scrumMasterId).CurrentGroup;
            var dbProject = _context.ScrumMasters.Find(scrumMasterId).CurrentGroup.Project;
            Assert.AreEqual(model.Group.TeamPurpose, dbGroup.TeamPurpose, "Group StartDate is not equal");
            Assert.AreEqual(model.Project.Name, dbProject.Name, "Project Name is not equal");
        }

        [TestMethod]
        public void ScrumMasterGetOldProjectAndModel_NotNull()
        {
            int scrumMasterId = _context.Groups.First(t => t.Project.Status != ProjectStatus.WaitingForDescription).ScrumMasterId.Value;
            var controller = CreateControllerWithFakeUser(scrumMasterId);

            RedirectToRouteResult result = controller.Project() as RedirectToRouteResult;
            Assert.IsNotNull(result, "View is null");
            object value = string.Empty;
            result.RouteValues.TryGetValue("controller", out value);
            Assert.AreEqual("Project", value.ToString(), "Url is not equal");
        }

        [TestMethod]
        public void ScrumMasterUpdateProject_NotNull()
        {
            var scrumMasterId = _context.Groups
                .Where(t => t.Project.Status == ProjectStatus.WaitingForDescription)
                .Select(g => g.ScrumMasterId.Value)
                .First();
            var controller = CreateControllerWithFakeUser(scrumMasterId);
            string teamPurpose = "testTeam";
            string fullDescription = "testFull";
            string shortDescription = "testShort";
            string teamworkDesciption = "testTeamDesc";
            var dbGroupId = _context.ScrumMasters.Where(sm => sm.Id == scrumMasterId).Select(sm => sm.GroupId.Value).Single();
            var dbProjectId = _context.Projects.Where(p => p.Groups.Any(g => g.Id == dbGroupId)).Select(p => p.Id).Single();
            
            var group = new GroupViewModel
            {
                Id = dbGroupId,
                TeamPurpose = teamPurpose,
                TeamWorkDescription = teamworkDesciption,
                ScrumMasterId = scrumMasterId,
                ProjectId = dbProjectId
            };

            var model = new ScrumProjectViewModel
            {
                Group = group,
                Project = new ProjectViewModel
                {
                    Id = dbProjectId,
                    FullDescription = fullDescription,
                    ShortDescription = shortDescription
                }
            };

            RedirectToRouteResult result = controller.Project(model) as RedirectToRouteResult;

            Assert.IsNotNull(result, "RedirectResult is null");
            Assert.AreEqual("Project", result.RouteValues["controller"], "Controller is not equal");
            Assert.AreEqual("View", result.RouteValues["action"], "Action is not equal");
            Assert.AreEqual(model.Project.Id, result.RouteValues["id"], "Id is not equal");

            var dbProject = _context.Projects.Single(p => p.Id == dbProjectId);
            var dbGroup = _context.Groups.Single(g => g.Id == group.Id);

            Assert.AreEqual(teamPurpose, dbGroup.TeamPurpose, "Group TeamPurpose is not equal");
            Assert.AreEqual(fullDescription, dbProject.FullDescription, "Project FullDescription is not equal");
            Assert.AreEqual(shortDescription, dbProject.ShortDescription, "Group ShortDescription is not equal");
            Assert.AreEqual(teamworkDesciption, dbGroup.TeamworkDescription, "Project TeamworkDesciption is not equal");
        }

        [TestMethod]
        public void ScrumMasterGetEditProjectAndModel_NotNull()
        {
            var projectId = _context.Projects.Where(t => t.Status == ProjectStatus.InProgress).First().Id;
            int scrumId = _context.Groups.Where(t => t.ProjectId == projectId).First().ScrumMasterId.Value;
            var controller = CreateControllerWithFakeUser(scrumId);
            ViewResult result = controller.Edit() as ViewResult;
            Assert.IsNotNull(result, "View is null");
            ScrumProjectViewModel model = result.ViewData.Model as ScrumProjectViewModel;
            Assert.IsNotNull(model, "Model is null");
            var dbGroup = _context.ScrumMasters.Find(scrumId).CurrentGroup;
            var dbProject = _context.ScrumMasters.Find(scrumId).CurrentGroup.Project;
            Assert.AreEqual(model.Group.TeamPurpose, dbGroup.TeamPurpose, "Group StartDate is not equal");
            Assert.AreEqual(model.Project.Name, dbProject.Name, "Project Name is not equal");
        }

        [TestMethod]
        public void ScrumMasterGetFakeEditProjectAndModel_NotNull()
        {
            var projectId = _context.Projects.Where(t => t.Status == ProjectStatus.WaitingForDescription).First().Id;
            int scrumId = _context.Groups.Where(t => t.ProjectId == projectId).First().ScrumMasterId.Value;
            var controller = CreateControllerWithFakeUser(scrumId);
            RedirectToRouteResult result = controller.Edit() as RedirectToRouteResult;
            Assert.IsNotNull(result, "View is null");
            object value = string.Empty;
            result.RouteValues.TryGetValue("controller", out value);
            Assert.AreEqual("Project", value.ToString(), "Url is not equal");
        }

        [TestMethod]
        public void ScrumMasterEditProject_NotNull()
        {
            var scrumMasterId = _context.Groups
                .Where(t => t.Project.Status == ProjectStatus.InProgress)
                .Select(g => g.ScrumMasterId.Value)
                .First();
            var controller = CreateControllerWithFakeUser(scrumMasterId);
            string teamPurpose = "testTeam";
            string fullDescription = "testFull";
            string shortDescription = "testShort";
            string teamworkDesciption = "testTeamDesc";
            var dbGroupId = _context.ScrumMasters.Where(sm => sm.Id == scrumMasterId).Select(sm => sm.GroupId.Value).Single();
            var dbProjectId = _context.Projects.Where(p => p.Groups.Any(g => g.Id == dbGroupId)).Select(p => p.Id).Single();

            var group = new GroupViewModel
            {
                Id = dbGroupId,
                TeamPurpose = teamPurpose,
                TeamWorkDescription = teamworkDesciption,
                ScrumMasterId = scrumMasterId,
                ProjectId = dbProjectId
            };

            var model = new ScrumProjectViewModel
            {
                Group = group,
                Project = new ProjectViewModel
                {
                    Id = dbProjectId,
                    FullDescription = fullDescription,
                    ShortDescription = shortDescription
                }
            };

            RedirectToRouteResult result = controller.Edit(model) as RedirectToRouteResult;

            Assert.IsNotNull(result, "RedirectResult is null");
            Assert.AreEqual("Project", result.RouteValues["controller"], "Controller is not equal");
            Assert.AreEqual("View", result.RouteValues["action"], "Action is not equal");
            Assert.AreEqual(model.Project.Id, result.RouteValues["id"], "Id is not equal");

            var dbProject = _context.Projects.Single(p => p.Id == dbProjectId);
            var dbGroup = _context.Groups.Single(g => g.Id == group.Id);

            Assert.AreEqual(teamPurpose, dbGroup.TeamPurpose, "Group TeamPurpose is not equal");
            Assert.AreEqual(fullDescription, dbProject.FullDescription, "Project FullDescription is not equal");
            Assert.AreEqual(shortDescription, dbProject.ShortDescription, "Group ShortDescription is not equal");
            Assert.AreEqual(teamworkDesciption, dbGroup.TeamworkDescription, "Project TeamworkDesciption is not equal");
        }

        private ScrumMasterController CreateControllerWithFakeUser(int scrumMasterId)
        {
            var fakeUser = new FakeUser(scrumMasterId);
            var fakeFactory = new FakeUserFactory(fakeUser);

            NewScope();
            return new ScrumMasterController(Kernel.Get<IScrumReviewProvider>(),
                                                    Kernel.Get<IScrumMasterProvider>(),
                                                    Kernel.Get<IGroupProvider>(),
                                                    Kernel.Get<IProjectProvider>(),
                                                    Kernel.Get<ScrumReviewMapper>(),
                                                    Kernel.Get<GroupMapper>(),
                                                    Kernel.Get<ProjectMapper>(),
                                                    fakeFactory);
        }
    }
}