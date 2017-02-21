using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StyleCop.SpecialRules.Tests
{
    [TestClass]
    public abstract class SourceAnalysisTest
    {
        private const string StyleCopSettingsFilePath = @"Settings.StyleCop";
        private const string TestFilesDirectory = "Test data";

        private CodeProject _project;

        protected SourceAnalysisTest()
        {
            var settings = Path.GetFullPath(StyleCopSettingsFilePath);
            var addinPaths = new List<string>();

            Console = new StyleCopConsole(settings, false, null, addinPaths, true);

            Console.ViolationEncountered +=
                (sender, args) => Violations.Add(args.Violation);

            Console.OutputGenerated +=
                (sender, args) => Output.Add(args.Output);
        }

        public StyleCopConsole Console { get; private set; }
        public List<string> Output { get; private set; }
        public List<Violation> Violations { get; private set; }

        [TestInitialize]
        public void Setup()
        {
            Violations = new List<Violation>();
            Output = new List<string>();

            var configuration = new Configuration(new string[0]);
            string location = Path.GetFullPath(StyleCopSettingsFilePath);

            _project = new CodeProject(Guid.NewGuid().GetHashCode(), location, configuration);
        }

        [TestCleanup]
        public void TearDown()
        {
            _project = null;
        }

        public void AddSourceCode(string fileName)
        {
            string relativePath = Path.Combine(TestFilesDirectory, fileName);
            string absolutePath = Path.GetFullPath(relativePath);

            if (!File.Exists(absolutePath)) Assert.Fail($"File '{absolutePath}' does not exist");

            Console.Core.Environment.AddSourceCode(_project, absolutePath, null);
        }

        public void StartAnalysis()
        {
            var projects = new[] { _project };

            Console.Start(projects, true);
        }

        public void AssertViolated(Predicate<Violation> match)
        {
            if (!Violations.Exists(match))
            {
                Assert.Fail("Failed to violate expected rule.");
            }
        }

        public void AssertNotViolated(string ruleName)
        {
            if (Violations.Exists(x => x.Rule.Name == ruleName))
            {
                Assert.Fail("Violated rule {0} by mistake.", ruleName);
            }
        }

        public void AssertNotViolated(SpecialRules rule)
        {
            AssertNotViolated(rule.ToString());
        }

        public void AssertViolated(string ruleName, int violationCount)
        {
            var actualCount = Violations.FindAll(x => x.Rule.Name == ruleName).Count;

            if (actualCount != violationCount)
            {
                Assert.Fail(
                    "Violated rule {0} {1} times instead of {2}.",
                    ruleName,
                    actualCount,
                    violationCount);
            }
        }

        public void AssertViolated(SpecialRules rule, int violationCount)
        {
            AssertViolated(rule.ToString(), violationCount);
        }

        public void AssertViolated(string ruleName, int[] lineNumbers)
        {
            if (lineNumbers != null &&
                lineNumbers.Length > 0)
            {
                foreach (var lineNumber in lineNumbers)
                {
                    if (!Violations.Exists(x =>
                        x.Rule.Name == ruleName &&
                        x.Line == lineNumber))
                    {
                        Assert.Fail(
                            "Failed to violate rule {0} on line {1}.",
                            ruleName,
                            lineNumber);
                    }
                }
            }
        }

        public void AssertViolated(SpecialRules rule, int[] lineNumbers)
        {
            AssertViolated(rule.ToString(), lineNumbers);
        }
    }
}
