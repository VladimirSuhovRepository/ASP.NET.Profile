using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.UI.Controllers;
using Profile.UI.Identity;
using Profile.UI.Mappers;
using Profile.UI.Models.Profile;
using Profile.UI.Tests.Infrastructure;
using Profile.UI.Tests.Ninject;

namespace Profile.UI.Tests.Controller
{
    [TestClass]
    public class ProfileControllerTest : DependencyInjectedTest
    {
        private IProfileContext _context;

        [TestInitialize]
        public void Setup()
        {
            NewScope();
            _context = Kernel.Get<IProfileContext>();
        }

        [TestMethod]
        public void ProfileGetViewAndModel_NotNull()
        {
            var controller = CreateProfileController(new FakeUserFactory());

            var id = _context.Trainees.First().Id;
            ViewResult result = controller.View(id) as ViewResult;
            Assert.IsNotNull(result, "View is null");
            ProfileViewModel model = result.ViewData.Model as ProfileViewModel;
            Assert.IsNotNull(model, "Model is null");
            Assert.IsNotNull(model.Job, "Job is null");
            var dbModel = _context.TraineeProfiles.Find(id);
            Assert.AreEqual(dbModel.CurrentPosition, model.Job.CurrentPosition, "CurrentPosition is not equal");
            Assert.AreEqual(dbModel.CurrentWork, model.Job.CurrentWork, "CurrentWork is not equal");
            Assert.AreEqual(dbModel.JobDuties, model.Job.JobDuties, "JobDuties is not equal");
            Assert.AreEqual(dbModel.DesirablePosition, model.Position.DesirablePosition, "DesirablePosition is not equal");
            Assert.IsTrue(model.Rating >= 0 && model.Rating <= 100, "Rating is out of range");
        }

        [TestMethod]
        public void ProfileEditAndModel_NotNull()
        {
            var fakeUser = new FakeUser(_context.Trainees.First().Id);
            var fakeFactory = new FakeUserFactory(fakeUser);

            var controller = CreateProfileController(fakeFactory);

            ViewResult result = controller.Edit() as ViewResult;
            Assert.IsNotNull(result, "View is null");
            ProfileViewModel model = result.ViewData.Model as ProfileViewModel;
            Assert.IsNotNull(model, "Model is null");
            Assert.IsNotNull(model.Job, "Job is null");
            var dbModel = _context.TraineeProfiles.Find(fakeUser.Id);
            Assert.AreEqual(dbModel.CurrentPosition, model.Job.CurrentPosition, "CurrentPosition is not equal");
            Assert.AreEqual(dbModel.CurrentWork, model.Job.CurrentWork, "CurrentWork is not equal");
            Assert.AreEqual(dbModel.JobDuties, model.Job.JobDuties, "JobDuties is not equal");
            Assert.AreEqual(dbModel.DesirablePosition, model.Position.DesirablePosition, "DesirablePosition is not equal");
            Assert.IsTrue(model.Rating >= 0 && model.Rating <= 100, "Rating is out of range");
        }

        private ProfileController CreateProfileController(ICurrentUserFactory fakeFactory)
        {
            NewScope();
            var profileProvider = Kernel.Get<IProfileProvider>();
            var profileMapper = Kernel.Get<ProfileMapper>();

            return new ProfileController(
                profileProvider,
                profileMapper,
                fakeFactory);
        }
    }
}
