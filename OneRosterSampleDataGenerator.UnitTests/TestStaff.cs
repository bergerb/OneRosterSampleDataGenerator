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
            Staff newTeacher = OneRoster.CreateStaff();

            Assert.IsNotNull(newTeacher);
            Assert.AreEqual(RoleType.teacher, newTeacher.RoleType);
            Assert.IsNull(newTeacher?.Org);
        }
        [TestMethod]
        public void TestNewTeacherWithOrg()
        {
            Staff newTeacher = OneRoster.CreateStaff(OneRoster.Orgs.First());

            Assert.IsNotNull(newTeacher);
            Assert.AreEqual(RoleType.teacher, newTeacher.RoleType);
            Assert.IsNotNull(newTeacher?.Org);
        }

        [TestMethod]
        public void TestTeachersOneEmailPerIdentifier()
        {
            // check for teacher email uniqueness in enrollments
            var OneRosterEmailCount = OneRoster.Staff.GroupBy(x => x.Email)
                .Where(x => x.Count() > 1)
                .Count();

            Assert.IsTrue(OneRosterEmailCount == 0);
        }

        [TestMethod]
        public void TestAtLeastOneAdminPerBuilding()
        {
            foreach (Org org in OneRoster.Orgs.Where(x => x.OrgType == OrgType.school))
            {
                var staffAdminCount = OneRoster.Staff
                      .Where(x => x.Org.Id == org.Id && x.RoleType == RoleType.administrator).Count();
                Assert.IsTrue(staffAdminCount > 0);
                // High
                if (org.isHigh)
                {
                    Assert.AreEqual(3, staffAdminCount);
                }
                // Middle
                if (org.isMiddle)
                {
                    Assert.AreEqual(2, staffAdminCount);
                }
                // Elementary
                if (org.isElementary)
                {
                    Assert.AreEqual(1, staffAdminCount);
                }
            }
        }
    }
}
