using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.UI.Controllers;
using Profile.UI.Models;
using Profile.UI.Tests.Infrastructure;
using Profile.UI.Tests.Ninject;
using Profile.UI.Utility;

namespace Profile.UI.Tests.Controller
{
    [TestClass]
    public class HomeControllerTest : DependencyInjectedTest
    {
        private HomeController _controller;
        private IProfileContext _context;

        [TestInitialize]
        public void Setup()
        {
            NewScope();
            var traineeProvider = Kernel.Get<ITraineeProvider>();
            var projectProvider = Kernel.Get<IProjectProvider>();
            var wordTranformer = Kernel.Get<WordTransformer>();
            var fakeFactory = new FakeUserFactory();

            _controller = new HomeController(
                traineeProvider,
                projectProvider,
                wordTranformer,
                fakeFactory);

            NewScope();
            _context = Kernel.Get<IProfileContext>();
        }

        [TestMethod]
        public void HomeGetIndexAndModel_NotNull()
        {
            ViewResult result = _controller.Index() as ViewResult;
            Assert.IsNotNull(result, "View is null");
            MetricsViewModel model = result.ViewData.Model as MetricsViewModel;
            Assert.IsNotNull(model, "Model is null");
            var projectsDb = _context.Projects.Count();
            var traineeDb = _context.Trainees.Count();
            Assert.AreEqual(model.ProjectsCount, projectsDb, "ProjectCount is not equal");
            Assert.AreEqual(model.TraineesCount, traineeDb, "TraineesCount is not equal");
            Assert.IsNotNull(model.ProjectsLabel, "ProjectsName is null");
            Assert.IsNotNull(model.ProjectsLabel, "TraineesName is null");
        }
    }
}
