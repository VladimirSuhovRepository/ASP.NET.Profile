using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Profile.DAL.Entities;
using Profile.UI.Utility;

namespace Profile.UI.Tests.Utility
{
    [TestClass]
    public class RoleComparerTest
    {
        [TestMethod]
        public void RoleComparer_ComparedCorrectly()
        {
            var exceptedResult = new List<RoleType>
            {
                RoleType.HR,
                RoleType.Manager,
                RoleType.ScrumMaster,
                RoleType.Mentor
            };

            var testData = new List<RoleType>
            {
                RoleType.ScrumMaster,
                RoleType.Mentor,
                RoleType.Manager,
                RoleType.HR
            };

            var roleComparer = new RoleComparer(new Dictionary<RoleType, int>
            {
                { RoleType.ScrumMaster, 3 },
                { RoleType.Mentor, 4 },
                { RoleType.Manager, 2 },
                { RoleType.HR, 1 }
            });

            var actualResult = testData.OrderBy(r => r, roleComparer).ToList();

            for (int i = 0; i < exceptedResult.Count; i++)
            {
                Assert.AreEqual(exceptedResult[i], actualResult[i], "Sequence is not sorted");
            }
        }
    }
}
