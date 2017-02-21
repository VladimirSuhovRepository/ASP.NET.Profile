using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Profile.DAL.Context;
using Profile.DAL.Entities;
using Profile.UI.Tests.Ninject;
using Profile.UI.Utility;

namespace Profile.UI.Tests.Utility
{
    [TestClass]
    public class SpecializationComparerTest : DependencyInjectedTest
    {
        private readonly IProfileContext _context;

        public SpecializationComparerTest()
        {
            _context = Kernel.Get<IProfileContext>();
        }

        [TestMethod]
        public void SpecializationComparer_ComparedCorrectly()
        {
            var exceptedResult = new List<string>(8);

            exceptedResult.Add(Specialization.BA);
            exceptedResult.Add(Specialization.Design);
            exceptedResult.Add(Specialization.Frontend);
            exceptedResult.Add(Specialization.BackendNET);
            exceptedResult.Add(Specialization.BackendAndroid);
            exceptedResult.Add(Specialization.BackendJava);
            exceptedResult.Add(Specialization.BackendPHP);
            exceptedResult.Add(Specialization.QA);

            var specializations = _context.Specializations.ToList();

            var actualResult = specializations
                .OrderBy(s => s, new SpecializationComparer())
                .ToList();

            var isSorted = true;

            for (int i = 0; i < exceptedResult.Count; i++)
            {
                if (string.Compare(exceptedResult[i], actualResult[i].Abbreviation) != 0)
                {
                    isSorted = false;
                    break;
                }
            }

            Assert.IsTrue(isSorted, "The sequense is not sorted correctly");
        }
    }
}
