using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Profile.UI.Tests.Ninject;
using Profile.UI.Utility;

namespace Profile.UI.Tests.Utility
{
    [TestClass]
    public class WordTransformerTest : DependencyInjectedTest
    {
        private WordTransformer _wordTransformer;
        
        [TestInitialize]
        public void Setup()
        {
            _wordTransformer = Kernel.Get<WordTransformer>();
        }

        [TestMethod]
        public void WordTransformerGetTraineeCasting_AreEqual()
        {
            Dictionary<int, string> testCases = new Dictionary<int, string>
             {
                 { 0, "Стажеров" },
                 { 1, "Стажер" },
                 { 2, "Стажера" },
                 { 4, "Стажера" },
                 { 5, "Стажеров" },
                 { 11, "Стажеров" },
                 { 12, "Стажеров" },
                 { 15, "Стажеров" },
                 { 20, "Стажеров" },
                 { 21, "Стажер" },
                 { 22, "Стажера" },
                 { 24, "Стажера" },
                 { 25, "Стажеров" },
                 { 101, "Стажер" },
                 { 111, "Стажеров" },
                 { 102, "Стажера" },
                 { 104, "Стажера" },
                 { 105, "Стажеров" },
                 { 120, "Стажеров" },
                 { 121, "Стажер" },
                 { 122, "Стажера" },
                 { 124, "Стажера" },
                 { 125, "Стажеров" },
                 { 212, "Стажеров" }
             };

            foreach (var item in testCases.Keys)
            {
                var result = _wordTransformer.GetNounCase(item, "Стажер");
                Assert.AreEqual(testCases[item], result, item + " Стажеров is not equal");
            }
        }

        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        [TestMethod]
        public void WordTransformerGetTraineeCasting_HighRangeException()
        {
            _wordTransformer.GetNounCase(1000, "Стажер");
        }

        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        [TestMethod]
        public void WordTransformerGetProjectCasting_HighRangeException()
        {
            _wordTransformer.GetNounCase(1000, "Проект");
        }

        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        [TestMethod]
        public void WordTransformerGetTraineeCasting_LowRangeException()
        {
            _wordTransformer.GetNounCase(-1, "Стажер");
        }

        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        [TestMethod]
        public void WordTransformerGetProjectCasting_LowRangeException()
        {
            _wordTransformer.GetNounCase(-1, "Проект");
        }

        [TestMethod]
        public void WordTransformerGetProjetCasting_AreEqual()
        {
            Dictionary<int, string> testCases = new Dictionary<int, string>
             {
                { 0, "Проектов" },
                 { 1, "Проект" },
                 { 2, "Проекта" },
                 { 4, "Проекта" },
                 { 5, "Проектов" },
                 { 11, "Проектов" },
                 { 12, "Проектов" },
                 { 15, "Проектов" },
                 { 20, "Проектов" },
                 { 21, "Проект" },
                 { 22, "Проекта" },
                 { 24, "Проекта" },
                 { 25, "Проектов" },
                 { 101, "Проект" },
                 { 111, "Проектов" },
                 { 102, "Проекта" },
                 { 104, "Проекта" },
                 { 105, "Проектов" },
                 { 120, "Проектов" },
                 { 121, "Проект" },
                 { 122, "Проекта" },
                 { 124, "Проекта" },
                 { 125, "Проектов" },
                 { 212, "Проектов" }
             };

            foreach (var item in testCases.Keys)
            {
                var result = _wordTransformer.GetNounCase(item, "Проект");
                Assert.AreEqual(testCases[item], result, item + " Проектов is not equal");
            }
        }
    }
}
