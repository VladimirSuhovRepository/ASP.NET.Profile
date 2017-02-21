using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StyleCop.SpecialRules.Tests
{
    [TestClass]
    public class SpecialAnalyzerTest : SourceAnalysisTest
    {
        [TestMethod]
        public void ConditionalStatements_CurlyBracketsMustNotBeOmitted_Invalid()
        {
            int exceptedViolationCount = 6;
            int[] exceptedViolationLineNumbers = { 10, 24, 33, 34, 44, 46 };

            AddSourceCode("ConditionalStatementsCurlyBracketsInvalid.cs");

            StartAnalysis();

            AssertViolated(SpecialRules.CurlyBracketsMustNotBeOmitted, exceptedViolationCount);
            AssertViolated(SpecialRules.CurlyBracketsMustNotBeOmitted, exceptedViolationLineNumbers);
        }

        [TestMethod]
        public void ConditionalStatements_CurlyBracketsMustNotBeOmitted_Valid()
        {
            AddSourceCode("ConditionalStatementsCurlyBracketsValid.cs");

            StartAnalysis();

            AssertNotViolated(SpecialRules.CurlyBracketsMustNotBeOmitted);
        }
    }
}
