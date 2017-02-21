using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;
using Profile.UI.Controllers;
using Profile.UI.Mappers;
using Profile.UI.Models.Project;
using Profile.UI.Tests.Infrastructure;
using Profile.UI.Tests.Ninject;
using Profile.UI.Utility;

namespace Profile.UI.Tests.Controller
{
    [TestClass]
    public class ProjectControllerTest : DependencyInjectedTest
    {
        private IProfileContext _context;
        private ProjectController _controller;
        private IProjectProvider _projectProvider;
        private ProjectMapper _projectMapper;

        [TestInitialize]
        public void Setup()
        {
            NewScope();
            _context = Kernel.Get<IProfileContext>();
            NewScope();
            _controller = Kernel.Get<ProjectController>();
            _projectProvider = Kernel.Get<IProjectProvider>();
            _projectMapper = Kernel.Get<ProjectMapper>();
        }

        [TestMethod]
        public void Project_View_ReturnedView_WhenProject_IsInProgress()
        {
            var fakeUser = new FakeUser(1);

            _controller = CreateProjectController(fakeUser);

            var project = GetTestProject();
            var result = _controller.View(project.Id) as ViewResult;

            Assert.IsNotNull(result, "Result is not a view");
        }

        [TestMethod]
        public void Project_View_Returned404_WhenProject_IsNot_InProgress()
        {
            var project = _context.Projects
                .Where(p => p.Status == ProjectStatus.WaitingForDescription)
                .FirstOrDefault();

            var result = _controller.View(project.Id) as HttpNotFoundResult;

            Assert.IsNotNull(result, "Result is not a HttpNotFoundResult");
        }

        [TestMethod]
        public void Project_View_GroupsAreSorted_ByStartDate_FromOld_ToNew()
        {
            var fakeUser = new FakeUser(1);

            _controller = CreateProjectController(fakeUser);

            var project = GetTestProject();
            var result = _controller.View(project.Id) as ViewResult;
            var viewModel = result.Model as ProjectDescriptionViewModel;
            var groups = viewModel.Groups.ToList();
            var isSorted = true;

            for (int i = 1; i < groups.Count; i++)
            {
                if (groups[i].StartDate < groups[i - 1].StartDate)
                {
                    isSorted = false;
                    break;
                }
            }

            Assert.IsTrue(isSorted, "Groups are not sorted by start date from old to new");
        }

        [TestMethod]
        public void Project_View_TraineesAreSorted_WithSpecializationComparer()
        {
            var project = GetTestProject();

            var result = _controller.View(project.Id) as ViewResult;
            var viewModel = result.Model as ProjectDescriptionViewModel;

            var actualGroup = viewModel.Groups.FirstOrDefault();
            var actualTraineeList = actualGroup.Trainees.ToList();
            var expectedGroup = _context.Groups.Find(actualGroup.Id);

            var expectedTraineeList = expectedGroup.Trainees
                .OrderBy(t => t.Specialization, new SpecializationComparer())
                .ToList();

            for (int i = 0; i < expectedTraineeList.Count; i++)
            {
                Assert.AreEqual(expectedTraineeList[i].Specialization.Abbreviation,
                                actualTraineeList[i].SpecializationAbbreviation,
                                "Trainees is not sorted with specialization comparer");
            }
        }

        [TestMethod]
        public void Project_View_Mapped_ViewModel_FullyAndCorrectly()
        {
            var fakeUser = new FakeUser(1);

            _controller = CreateProjectController(fakeUser);

            var project = GetTestProject();
            var result = _controller.View(project.Id) as ViewResult;
            var viewModel = result.Model as ProjectDescriptionViewModel;

            var groupViewModel = viewModel.Groups.FirstOrDefault();
            var group = _context.Groups.Find(groupViewModel.Id);

            var scrumViewModel = groupViewModel.ScrumMaster;
            var scrum = group.ScrumMaster;

            var traineeViewModel = groupViewModel.Trainees.FirstOrDefault();
            var trainee = _context.Trainees.Find(traineeViewModel.Id);

            // Checking project data
            Assert.AreEqual(project.Name, viewModel.Name, "Name properties are different");

            Assert.AreEqual(project.FullDescription, 
                            viewModel.FullDescription, 
                            "FullDescription properties are different");

            Assert.AreEqual(project.StartDate, 
                            viewModel.StartDate, 
                            "StartDate properties are different");

            Assert.AreEqual(project.FinishDate, 
                            viewModel.FinishDate, 
                            "FinishDate properties are different");

            int expectedGroupCount = project.Groups.Count(g => g.HasScrumMaster);

            Assert.AreEqual(expectedGroupCount,
                            viewModel.Groups.Count,
                            "Group counts are different");

            Assert.AreEqual(project.IsScrumMasterOwner(fakeUser.Id.Value),
                            viewModel.IsCurrentUserOwner,
                            "IsUserOwner and IsCurrentUserOwner properties are different");

            // Checking group data
            Assert.AreEqual(group.Number, groupViewModel.Number, "Number properties are different");

            Assert.AreEqual(group.StartDate, 
                            groupViewModel.StartDate, 
                            "StartDate properties are different");

            Assert.AreEqual(group.FinishDate, 
                            groupViewModel.FinishDate,
                            "FinishDate properties are different");

            Assert.AreEqual(group.TeamPurpose,
                            groupViewModel.TeamPurpose,
                            "TeamPurpose properties are different");

            Assert.AreEqual(group.TeamworkDescription,
                            groupViewModel.TeamWorkDescription,
                            "TeamworkDesciption properties are different");

            Assert.AreEqual(group.Trainees.Count,
                            groupViewModel.Trainees.Count,
                            "Trainee counts are different");

            // Checking scrum data
            Assert.AreEqual(scrum.User.FullName,
                            scrumViewModel.UserFullName,
                            "Scrum fullname properties are different");

            // Checking trainee data
            Assert.AreEqual(trainee.User.FullName,
                            traineeViewModel.UserFullName,
                            "Trainee fullname properties are different");

            Assert.AreEqual(trainee.Specialization.Abbreviation,
                            traineeViewModel.SpecializationAbbreviation,
                            "SpecializationAbbreviation properties are different");

            Assert.AreEqual(trainee.IsAllowed,
                            traineeViewModel.IsAllowed,
                            "IsAllowed properties are different");
        }

        [TestMethod]
        public void Project_View_IsCurrentUserOwner_IsFalse_WhenUser_IsNotOwner()
        {
            var project = GetTestProject();
            var trainee = _context.Trainees.First();
            var fakeUser = new FakeUser(trainee.Id);

            fakeUser.AddRole(RoleType.Trainee);
            _controller = CreateProjectController(fakeUser);

            var result = _controller.View(project.Id) as ViewResult;
            var viewModel = result.Model as ProjectDescriptionViewModel;

            Assert.IsFalse(viewModel.IsCurrentUserOwner, "This user is project owner");
        }

        [TestMethod]
        public void Project_View_IsCurrentUserOwner_IsTrue_WhenUser_IsOwner()
        {
            var project = GetTestProject();
            int scrumId = project.Groups.First(g => g.ScrumMasterId.HasValue).ScrumMasterId.Value;
            var fakeUser = new FakeUser(scrumId);

            fakeUser.AddRole(RoleType.ScrumMaster);
            _controller = CreateProjectController(fakeUser);

            var result = _controller.View(project.Id) as ViewResult;
            var viewModel = result.Model as ProjectDescriptionViewModel;

            Assert.IsTrue(viewModel.IsCurrentUserOwner, "This user is not project owner");
        }

        private ProjectController CreateProjectController(FakeUser fakeUser)
        {
            var fakeFactory = new FakeUserFactory(fakeUser);

            return new ProjectController(_projectProvider, _projectMapper, fakeFactory);
        }

        private Project GetTestProject()
        {
            int minTraineeCount = 3;
            int minGroupCount = 3;
            string projectIsNotExistingMessage = "Cannot perform the test because there is no project which satisfies the condition";

            var project = _context.Projects
                .Where(p => p.Status == ProjectStatus.InProgress && 
                    p.Groups.Count >= minGroupCount &&
                    p.Groups.Any(g => g.Trainees.Count >= minTraineeCount))
                .FirstOrDefault();

            if (project == null) Assert.Fail(projectIsNotExistingMessage);

            return project;
        }
    }
}
