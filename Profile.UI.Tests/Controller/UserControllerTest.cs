using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;
using Profile.UI.Controllers;
using Profile.UI.Infrastructure.DetailedInfo;
using Profile.UI.Mappers;
using Profile.UI.Models.User;
using Profile.UI.Tests.Infrastructure;

namespace Profile.UI.Tests.Controller
{
    [TestClass]
    public class UserControllerTest : TransactionalTest
    {
        private readonly IProfileContext _context;

        public UserControllerTest()
        {
            NewScope();
            _context = Kernel.Get<IProfileContext>();
        }

        [TestMethod]
        public async Task ViewAction_MappedCorrectly()
        {
            int expectedDetailsCount = 1;
            string expectedViewName = "_MentorDetails";

            var viewingUser = GetUserWithSpecifiedRoles(RoleType.Trainee);
            var viewedUser = GetUserWithSpecifiedRoles(RoleType.Mentor);

            var fakeUser = new FakeUser(viewingUser.Id);
            var controller = CreateController(fakeUser);

            var result = await controller.View(viewedUser.Id) as ViewResult;

            Assert.IsNotNull(result, "It is not a view");

            var viewModel = result.Model as UserProfile;

            Assert.IsNotNull(result, "View model is not a UserProfile");
            Assert.AreEqual(viewedUser.Id, viewModel.Id, "Ids are different");
            Assert.AreEqual(viewedUser.FullName, viewModel.FullName, "Fullnames are different");
            Assert.IsFalse(viewModel.IsOwner, "IsOwner is mapped incorrectly");

            var contacts = viewedUser.Contacts;
            var viewContacts = viewModel.Contacts;

            Assert.IsNotNull(contacts, "User does not have contacts. Cannot continue the test");
            Assert.IsNotNull(viewContacts, "View model does not have contacts");

            Assert.AreEqual(contacts.Email, viewContacts.Email, "Emails are different");
            Assert.AreEqual(contacts.Phone, viewContacts.Phone, "Phones are different");
            Assert.AreEqual(contacts.Skype, viewContacts.Skype, "Skype properties are different");
            Assert.AreEqual(contacts.LinkedIn, viewContacts.LinkedIn, "LinkedIn propertuis are different");
            Assert.AreEqual(contacts.Company, viewContacts.Company, "Companies are different");

            Assert.IsNotNull(viewModel.Details, "Details is null");

            Assert.AreEqual(expectedDetailsCount,
                            viewModel.Details.Count,
                            "Details counts are different");

            var details = viewModel.Details.Single() as DetailedMentorInfo;
            var mentor = await _context.Mentors.FindAsync(viewedUser.Id);

            Assert.IsNotNull(details, "It is not a DetailedMentorInfo");
            Assert.AreEqual(expectedViewName, details.PartialViewName, "View names are different");

            Assert.AreEqual(mentor.Specialization.Name,
                            details.SpecializationName,
                            "Specialization names are different");
        }

        [TestMethod]
        public async Task ViewAction_IsOwnerIsFalse_WhenUserIsNotOwner()
        {
            var viewingUser = GetUserWithSpecifiedRoles(RoleType.Trainee);
            var viewedUser = GetUserWithSpecifiedRoles(RoleType.Mentor);
            bool expectedIsOwner = false;

            await TestOwnership(viewingUser, expectedIsOwner, viewedUser);
        }

        [TestMethod]
        public async Task ViewAction_IsOwnerIsTrue_WhenUserIsOwner()
        {
            var viewingUser = GetUserWithSpecifiedRoles(RoleType.Mentor);
            bool expectedIsOwner = true;

            await TestOwnership(viewingUser, expectedIsOwner, viewingUser);
        }

        [TestMethod]
        public async Task ViewAction_IsOwnerIsTrue_WhenIdIsNull()
        {
            var viewingUser = GetUserWithSpecifiedRoles(RoleType.Mentor);
            bool expectedIsOwner = true;

            await TestOwnership(viewingUser, expectedIsOwner);
        }

        [TestMethod]
        public async Task ViewAction_ReturnedCorrectedDetails_ForManager()
        {
            int expectedDetailsCount = 1;
            string expectedViewName = "_ManagerDetails";
            var user = GetUserWithSpecifiedRoles(RoleType.Manager);
            var fakeUser = new FakeUser(user.Id);

            fakeUser.AddRole(RoleType.Manager);

            var userDetails = await GetTestedUserDetails(user, fakeUser, expectedDetailsCount);
            var managerDetails = userDetails.Single() as DetailedManagerInfo;

            Assert.IsNotNull(managerDetails, "It is not a DetailedManagerInfo");
            Assert.AreEqual(expectedViewName, managerDetails.PartialViewName, "View names are different");
        }

        [TestMethod]
        public async Task ViewAction_ReturnedCorrectedDetails_ForHR()
        {
            int expectedDetailsCount = 1;
            string expectedViewName = "_HRDetailsView";
            var user = GetUserWithSpecifiedRoles(RoleType.HR);
            var fakeUser = new FakeUser(user.Id);

            fakeUser.AddRole(RoleType.HR);

            var userDetails = await GetTestedUserDetails(user, fakeUser, expectedDetailsCount);
            var hrDetails = userDetails.Single() as DetailedHRInfo;
            var hr = _context.Users.Find(user.Id);

            Assert.IsNotNull(hrDetails, "It is not a DetailedHRInfo");
            Assert.AreEqual(expectedViewName, hrDetails.PartialViewName, "View names are different");

            string expectedCompany = hr.Contacts?.Company;

            Assert.AreEqual(expectedCompany,
                            hrDetails.Company,
                            "Companies are different");
        }

        [TestMethod]
        public async Task ViewAction_ReturnedCorrectedDetails_ForMentor()
        {
            int expectedDetailsCount = 1;
            string expectedViewName = "_MentorDetails";
            var user = GetUserWithSpecifiedRoles(RoleType.Mentor);
            var fakeUser = new FakeUser(user.Id);

            fakeUser.AddRole(RoleType.Mentor);

            var userDetails = await GetTestedUserDetails(user, fakeUser, expectedDetailsCount);
            var mentorDetails = userDetails.Single() as DetailedMentorInfo;
            var mentor = await _context.Mentors.FindAsync(user.Id);

            Assert.IsNotNull(mentorDetails, "It is not a DetailedMentorInfo");
            Assert.AreEqual(expectedViewName, mentorDetails.PartialViewName, "View names are different");

            Assert.AreEqual(mentor.Specialization.Name,
                            mentorDetails.SpecializationName,
                            "Specializations are different");
        }

        [TestMethod]
        public async Task ViewAction_ReturnedCorrectedDetails_ForScrumMaster()
        {
            int expectedDetailsCount = 1;
            string expectedViewName = "_ScrumMasterDetails";
            var user = GetUserWithSpecifiedRoles(RoleType.ScrumMaster);
            var fakeUser = new FakeUser(user.Id);

            fakeUser.AddRole(RoleType.ScrumMaster);

            var userDetails = await GetTestedUserDetails(user, fakeUser, expectedDetailsCount);
            var smDetails = userDetails.Single() as DetailedScrumMasterInfo;
            var sm = await _context.ScrumMasters.FindAsync(user.Id);

            Assert.IsNotNull(smDetails, "It is not a DetailedScrumMasterInfo");
            Assert.AreEqual(expectedViewName, smDetails.PartialViewName, "View names are different");

            string expectedProjectName = sm.CurrentGroup?.Project.Name;

            Assert.AreEqual(expectedProjectName,
                            smDetails.ProjectName,
                            "Projects are different");
        }

        [TestMethod]
        public async Task ViewAction_ReturnedCorrectedDetails_ForScrumMasterMentor()
        {
            int expectedDetailsCount = 2;
            string expectedMentorViewName = "_MentorDetails";
            string expectedScrumViewName = "_ScrumMasterDetails";

            var user = GetUserWithSpecifiedRoles(RoleType.ScrumMaster, RoleType.Mentor);
            var fakeUser = new FakeUser(user.Id);

            fakeUser.AddRole(RoleType.ScrumMaster);
            fakeUser.AddRole(RoleType.Mentor);

            var userDetails = await GetTestedUserDetails(user, fakeUser, expectedDetailsCount);
            var mentorDetails = userDetails[0] as DetailedMentorInfo;
            var smDetails = userDetails[1] as DetailedScrumMasterInfo;

            var mentor = await _context.Mentors.FindAsync(user.Id);
            var sm = await _context.ScrumMasters.FindAsync(user.Id);

            Assert.IsNotNull(mentorDetails, "It is not a DetailedMentorInfo");

            Assert.AreEqual(expectedMentorViewName, 
                            mentorDetails.PartialViewName, 
                            "View names are different");

            Assert.AreEqual(mentor.Specialization.Name,
                            mentorDetails.SpecializationName,
                            "Specializations are different");

            Assert.IsNotNull(smDetails, "It is not a DetailedScrumMasterInfo");

            Assert.AreEqual(expectedScrumViewName, 
                            smDetails.PartialViewName, 
                            "View names are different");

            string expectedProjectName = sm.CurrentGroup?.Project.Name;

            Assert.AreEqual(expectedProjectName,
                            smDetails.ProjectName,
                            "Projects are different");
        }

        private UserController CreateController(FakeUser fakeUser)
        {
            NewScope();

            var fakeFactory = new FakeUserFactory(fakeUser);

            var userProvider = Kernel.Get<IUsersProvider>();
            var roleProvider = Kernel.Get<IRoleProvider>();
            var detailsBuilder = Kernel.Get<IDetailedInfoBuilder>();
            var userMapper = Kernel.Get<UserMapper>();

            return new UserController(fakeFactory,
                                      userProvider,
                                      roleProvider,
                                      detailsBuilder,
                                      userMapper);
        }

        private User GetUserWithSpecifiedRoles(params RoleType[] roleTypes)
        {
            return GetUsersWithSpecifiedRoles(roleTypes).First();
        }

        private IList<User> GetUsersWithSpecifiedRoles(params RoleType[] roleTypes)
        {
            var roleIds = new List<int>();

            foreach (var roleType in roleTypes)
            {
                var role = _context.Roles.SingleOrDefault(r => r.Type == roleType);

                Assert.IsNotNull(role, $"{roleType} does not exist");

                roleIds.Add(role.Id);
            }

            var users = _context.Users.Where(
                u => roleIds.All(
                    i => u.Roles.Any(
                        r => r.RoleId == i)))
                .ToList();

            Assert.AreNotEqual(0, users.Count, "User with specified roles does not exist");

            return users;
        }

        private async Task TestOwnership(User viewingUser,
                                         bool expectedIsOwner,
                                         User viewedUser = null)
        {
            var fakeUser = new FakeUser(viewingUser.Id);
            var controller = CreateController(fakeUser);

            var result = await controller.View(viewedUser?.Id) as ViewResult;

            Assert.IsNotNull(result, "It is not a view");

            var viewModel = result.Model as UserProfile;

            Assert.IsNotNull(result, "View model is not a UserProfile");
            Assert.AreEqual(expectedIsOwner, viewModel.IsOwner, "IsOwner value is incorrect");
        }

        private async Task<IList<IDetailedInfo>> GetTestedUserDetails(User user, 
                                                                      FakeUser fakeUser,
                                                                      int expectedDetailsCount)
        {
            var controller = CreateController(fakeUser);

            var result = await controller.View(user.Id) as ViewResult;

            Assert.IsNotNull(result, "It is not a view");

            var viewModel = result.Model as UserProfile;

            Assert.IsNotNull(result, "View model is not a UserProfile");
            Assert.IsNotNull(viewModel.Details, "Details is null");

            Assert.AreEqual(expectedDetailsCount,
                            viewModel.Details.Count,
                            "Details counts are different");

            return viewModel.Details;
        }
    }
}
