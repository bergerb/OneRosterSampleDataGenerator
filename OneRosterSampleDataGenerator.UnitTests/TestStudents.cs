using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace OneRosterSampleDataGenerator.UnitTests
{
    /// <summary>
    /// Testing Student Objects
    /// </summary>
    [TestClass]
    public class TestStudents
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
        public void TestStudentsAvailable()
        {
            // check for students
            Assert.IsTrue(OneRoster.Students.Count() > 0);
        }
        /// <summary>
        /// Every student e-mail must have an @ symbol
        /// </summary>
        /// <todo>
        /// TODO: Expand to e-mail REGEX
        /// </todo>
        [TestMethod]
        public void TestEnsureEmail()
        {
            // pick a random student -> test
            Assert.IsTrue(OneRoster.Students[rnd.Next(0,OneRoster.Students.Count()-1)].Email.Contains("@"));
        }
        /// <summary>
        /// There are no duplicates allowed based on identifier
        /// </summary>
        [TestMethod]
        public void TestEnsureNoDupes()
        {
            var students = from s in OneRoster.Students
                           group s.Identifier by s.Identifier into dupes
                           where dupes.Count() > 1
                           select new { identifier = dupes.Key, count = dupes.Count() };

            Assert.IsTrue(students.Count() == 0);
        }

        /// <summary>
        /// Every student must belong to an organization (school)
        /// </summary>
        [TestMethod]
        public void TestEnsureOrgs()
        {
            Assert.IsTrue(OneRoster.Students.Where(e => e.Org == null).Count() == 0);
        }
    }
}
