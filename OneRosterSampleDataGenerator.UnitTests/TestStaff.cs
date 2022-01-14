using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneRosterSampleDataGenerator.Models;
using System.Linq;

namespace OneRosterSampleDataGenerator.UnitTests
{
    [TestClass]
    public class TestStaff
    {
        OneRoster OneRoster;

        [TestInitialize()]
        public void TestStaffInitialize()
        {
            OneRoster = new OneRoster();
        }

        [TestMethod]
        public void TestNewTeacher()
        {
            Teacher newTeacher = OneRoster.CreateTeacher();

            Assert.IsNotNull(newTeacher);
            Assert.IsNull(newTeacher?.org);
        }
        [TestMethod]
        public void TestNewTeacherWithOrg()
        {
            Teacher newTeacher = OneRoster.CreateTeacher(OneRoster.orgs.First());

            Assert.IsNotNull(newTeacher);
            Assert.IsNotNull(newTeacher?.org);
        }

        [TestMethod]
        public void TestTeachersOneEmailPerIdentifier()
        {
            // check for teacher email uniqueness in enrollments
            var OneRosterEmailCount = OneRoster.teachers.GroupBy(x => x.email)
                .Where(x => x.Count() > 1)
                .Count();

            Assert.IsTrue(OneRosterEmailCount == 0);
        }
    }
}
