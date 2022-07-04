using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    /// <summary>
    /// Testing Student Objects
    /// </summary>
    public class TestAcademicsSessions
    {

        OneRoster OneRoster;

        [SetUp]
        public void TestStaffInitialize()
        {
            OneRoster = new OneRoster();
        }

        /// <summary>
        /// There must be students available
        /// </summary>
        [Test]
        public void TestAcademicSessionsAvailable()
        {
            // check for students
            OneRoster.AcademicSessions.Count.ShouldBeGreaterThan(0);
        }

        private static readonly object[] currentYearTests =
        {
            new object[] { "12/1/2019", "2019" },
            new object[] { "1/1/2020", "2019" },
            new object[] { "1/1/2020", "2019" },
            new object[] { "12/1/2020", "2020" },
            new object[] { "7/1/2021", "2020" },
            new object[] { "12/1/2021", "2021" },
        };
        /// <summary>
        /// Test Business Logic for Current School Year Generation
        /// </summary>
        [TestCaseSource(nameof(currentYearTests))]
        public void TestCurrentYear(string dateToCheck, string year)
        {
            Utility.GetCurrentSchoolYear(DateTime.Parse(dateToCheck)).ShouldBe(year);
        }

        private static readonly object[] nextYearTests = 
        {
            new object[] { "12/1/2019", "2020" },
            new object[] { "1/1/2020", "2020" },
            new object[] { "1/1/2020", "2020" },
            new object[] { "12/1/2020", "2021" },
            new object[] { "7/1/2021", "2021" },
            new object[] { "12/1/2021", "2022" },
        };
        /// <summary>
        /// Test Business Logic for Next School Year Generation
        /// </summary>
        [TestCaseSource(nameof(nextYearTests))]
        public void TestNextYear(string dateToCheck, string year)
        {
            Utility.GetNextSchoolYear(DateTime.Parse(dateToCheck)).ShouldBe(year);
        }
    }
}
