using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Profile.DAL.Context;
using Profile.UI.Controllers;
using Profile.UI.Models.Review;
using Profile.UI.Tests.Ninject;

namespace Profile.UI.Tests.Controller
{
    [TestClass]
    public class ReviewControllerTest : DependencyInjectedTest
    {
        private ReviewController _controller;
        private IProfileContext _context;

        [TestInitialize]
        public void Setup()
        {
            NewScope();
            _controller = Kernel.Get<ReviewController>();
            NewScope();
            _context = Kernel.Get<IProfileContext>();
        }

        [TestMethod]
        public void ReviewGetViewAndModel_NotNull()
        {
            var traineeWithReviews = _context.Trainees.FirstOrDefault(t => t.ReviewsOnMe.Any());

            if (traineeWithReviews == null)
            {
                Assert.Fail("Database has no needed data for this test");
            }

            ViewResult result = _controller.View(traineeWithReviews.Id) as ViewResult;
            Assert.IsNotNull(result, "View is null");
            ProfileReviewViewModel model = result.ViewData.Model as ProfileReviewViewModel;
            Assert.IsNotNull(model, "Model is null");
            Assert.IsTrue(model.Rating >= 0 && model.Rating <= 100, "Rating is out of range");
            Assert.IsNotNull(model.IsTraineeHavingMentorReview, "IsTraineeHavingMentorReview is null");
            Assert.IsNotNull(model.IsTraineeHavingScrumReview, "IsTraineeHavingScrumReview is null");
            Assert.IsNotNull(model.IsTraineeHavingTeamReview, "IsTraineeHavingTeamReview is null");
        }
    }
}