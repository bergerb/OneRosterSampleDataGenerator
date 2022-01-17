using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace OneRosterSampleDataGenerator.UnitTests
{
    [TestClass]
    public class TestEnrollments
    {
        OneRoster OneRoster;

        [TestInitialize()]
        public void TestStaffInitialize()
        {
            OneRoster = new OneRoster();
        }
        [TestMethod]
        public void TestTeachersAvailable()
        {
            // check for teacher enrollments
            Assert.IsTrue(OneRoster.Enrollments.Where(e => e.RoleType == Models.RoleType.teacher).Count() > 0);
        }
        [TestMethod]
        public void TestStudentsAvailable()
        {
            // check for teacher enrollments
            Assert.IsTrue(OneRoster.Enrollments.Where(e => e.RoleType == Models.RoleType.student).Count() > 0);
        }
    }
}
