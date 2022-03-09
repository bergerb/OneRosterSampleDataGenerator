using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;
using System;
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

        /// <summary>
        /// Test Business Logic for Current School Year Generation
        /// </summary>
        [Test]
        public void TestCurrentYear()
        {
            Utility.GetCurrentSchoolYear(DateTime.Parse("12/1/2019")).ShouldBe("2019");
            Utility.GetCurrentSchoolYear(DateTime.Parse("1/1/2020")).ShouldBe("2019");

            Utility.GetCurrentSchoolYear(DateTime.Parse("1/1/2020")).ShouldBe("2019");
            Utility.GetCurrentSchoolYear(DateTime.Parse("12/1/2020")).ShouldBe("2020");

            Utility.GetCurrentSchoolYear(DateTime.Parse("7/1/2021")).ShouldBe("2021");
            Utility.GetCurrentSchoolYear(DateTime.Parse("12/1/2021")).ShouldBe("2021");
        }

        /// <summary>
        /// Test Business Logic for Next School Year Generation
        /// </summary>
        [Test]
        public void TestNextYear()
        {
            Utility.GetNextSchoolYear(DateTime.Parse("12/1/2019")).ShouldBe("2020");
            Utility.GetNextSchoolYear(DateTime.Parse("1/1/2020")).ShouldBe("2020");

            Utility.GetNextSchoolYear(DateTime.Parse("1/1/2020")).ShouldBe("2020");
            Utility.GetNextSchoolYear(DateTime.Parse("12/1/2020")).ShouldBe("2021");

            Utility.GetNextSchoolYear(DateTime.Parse("7/1/2021")).ShouldBe("2022");
            Utility.GetNextSchoolYear(DateTime.Parse("12/1/2021")).ShouldBe("2022");
        }
    }
}
