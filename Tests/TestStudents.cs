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
    public class TestStudents
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
        public void TestStudentsAvailable()
        {
            // check for students
            OneRoster.Students.Count.ShouldBeGreaterThan(0);
        }

        /// <summary>
        /// Every student e-mail must have an @ symbol
        /// </summary>
        /// <todo>
        /// TODO: Expand to e-mail REGEX
        /// </todo>
        [Test]
        public void TestEnsureEmail()
        {
            // pick a random student -> test
            var random = new Bogus.Randomizer();
            var rndStudent = random.Number(0, OneRoster.Students.Count() - 1);
            OneRoster.Students[rndStudent].Email.ShouldContain("@");
        }

        /// <summary>
        /// There are no duplicates allowed based on identifier
        /// </summary>
        [Test]
        public void TestEnsureNoDupes()
        {
            var students = (from s in OneRoster.Students
                           group s.Identifier by s.Identifier into dupes
                           where dupes.Count() > 1
                           select new { identifier = dupes.Key, count = dupes.Count() })
                           .ToList();

            students.Count.ShouldBe(0);
        }

        /// <summary>
        /// Every student must belong to an organization (school)
        /// </summary>
        [Test]
        public void TestEnsureOrgs()
        {
            Assert.IsTrue(OneRoster.Students.Where(e => e.Org == null).Count() == 0);
            OneRoster.Students.Count(e => e.Org == null).ShouldBe(0);
        }
    }
}
