using NUnit.Framework;
using OneRosterSampleDataGenerator;
using OneRosterSampleDataGenerator.Models;
using Shouldly;
using System.Linq;

namespace Tests
{
    public class TestEnrollments
    {
        OneRoster OneRoster;

        [SetUp]
        public void TestStaffInitialize()
        {
            OneRoster = new OneRoster();
        }
        [Test]
        public void TestTeachersAvailable()
        {
            // check for teacher enrollments
            OneRoster.Enrollments.Count(e => e.RoleType == RoleType.teacher).ShouldBeGreaterThan(0);
        }
        [Test]
        public void TestStudentsAvailable()
        {
            // check for teacher enrollments
            OneRoster.Enrollments.Count(e => e.RoleType == RoleType.student).ShouldBeGreaterThan(0);
        }
    }
}
