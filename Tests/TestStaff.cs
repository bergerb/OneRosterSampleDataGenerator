using NUnit.Framework;
using OneRosterSampleDataGenerator;
using OneRosterSampleDataGenerator.Models;
using Shouldly;
using System.Linq;

namespace Tests
{
    public class TestStaff
    {
        OneRoster OneRoster;

        [SetUp]
        public void TestStaffInitialize()
        {
            OneRoster = new OneRoster();
        }

        [Test]
        public void TestNewTeacher()
        {
            Staff newTeacher = OneRoster.CreateStaff();

            newTeacher.ShouldNotBeNull();
            newTeacher.RoleType.ShouldBe(RoleType.teacher);
            newTeacher.Org.ShouldBeNull();
        }

        [Test]
        public void TestNewTeacherWithOrg()
        {
            Staff newTeacher = OneRoster.CreateStaff(OneRoster.Orgs.First());

            newTeacher.ShouldNotBeNull();
            newTeacher.RoleType.ShouldBe(RoleType.teacher);
            newTeacher.Org.ShouldNotBeNull();
        }

        [Test]
        public void TestTeachersOneEmailPerIdentifier()
        {
            // check for teacher email uniqueness in enrollments
            var OneRosterEmailCount = OneRoster.Staff.GroupBy(x => x.Email)
                .Where(x => x.Count() > 1)
                .Count();

            OneRosterEmailCount.ShouldBe(0);
        }

        [Test]
        public void TestAtLeastOneAdminPerBuilding()
        {
            foreach (Org org in OneRoster.Orgs.Where(x => x.OrgType == OrgType.school))
            {
                var staffAdminCount = OneRoster.Staff
                      .Where(x => x.Org.Id == org.Id && x.RoleType == RoleType.administrator).Count();
                Assert.IsTrue(staffAdminCount > 0);
                // High
                if (org.IsHigh)
                {
                    staffAdminCount.ShouldBe(3);
                }
                // Middle
                if (org.IsMiddle)
                {
                    staffAdminCount.ShouldBe(2);
                }
                // Elementary
                if (org.IsElementary)
                {
                    staffAdminCount.ShouldBe(1);
                }
            }
        }
    }
}
