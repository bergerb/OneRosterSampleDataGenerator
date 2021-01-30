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
            Assert.IsTrue(OneRoster.enrollments.Where(e => e.role == "teacher").Count() > 0);
        }
        [TestMethod]
        public void TestStudentsAvailable()
        {
            // check for teacher enrollments
            Assert.IsTrue(OneRoster.enrollments.Where(e => e.role == "student").Count() > 0);
        }
    }
}
