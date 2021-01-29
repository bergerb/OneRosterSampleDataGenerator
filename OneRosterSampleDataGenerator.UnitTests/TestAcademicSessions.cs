using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace OneRosterSampleDataGenerator.UnitTests
{
    /// <summary>
    /// Testing Student Objects
    /// </summary>
    [TestClass]
    public class TestAcademicsSessions
    {

        OneRoster OneRoster;
        Random rnd = new Random();

        [TestInitialize()]
        public void TestStaffInitialize()
        {
            OneRoster = new OneRoster();
        }

        /// <summary>
        /// There must be students available
        /// </summary>
        [TestMethod]
        public void TestAcademicSessionsAvailable()
        {
            // check for students
            Assert.IsTrue(OneRoster.academicSessions.Count() > 0);
        }

        /// <summary>
        /// Test Business Logic for Current School Year Generation
        /// </summary>
        [TestMethod]
        public void TestCurrentYear()
        {
            Assert.IsTrue(Utility.GetCurrentSchoolYear(DateTime.Parse("12/1/2019")) == "2019");
            Assert.IsTrue(Utility.GetCurrentSchoolYear(DateTime.Parse("1/1/2020")) == "2019");
            
            Assert.IsTrue(Utility.GetCurrentSchoolYear(DateTime.Parse("1/1/2020")) == "2019");
            Assert.IsTrue(Utility.GetCurrentSchoolYear(DateTime.Parse("12/1/2020")) == "2020");

            Assert.IsTrue(Utility.GetCurrentSchoolYear(DateTime.Parse("7/1/2021")) == "2021");
            Assert.IsTrue(Utility.GetCurrentSchoolYear(DateTime.Parse("12/1/2021")) == "2021");
        }

        /// <summary>
        /// Test Business Logic for Next School Year Generation
        /// </summary>
        [TestMethod]
        public void TestNextYear()
        {
            Assert.IsTrue(Utility.GetNextSchoolYear(DateTime.Parse("12/1/2019")) == "2020");
            Assert.IsTrue(Utility.GetNextSchoolYear(DateTime.Parse("1/1/2020")) == "2020");

            Assert.IsTrue(Utility.GetNextSchoolYear(DateTime.Parse("1/1/2020")) == "2020");
            Assert.IsTrue(Utility.GetNextSchoolYear(DateTime.Parse("12/1/2020")) == "2021");

            Assert.IsTrue(Utility.GetNextSchoolYear(DateTime.Parse("7/1/2021")) == "2022");
            Assert.IsTrue(Utility.GetNextSchoolYear(DateTime.Parse("12/1/2021")) == "2022");
        }

    }
}
