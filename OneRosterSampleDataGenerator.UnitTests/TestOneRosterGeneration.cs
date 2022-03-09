using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace OneRosterSampleDataGenerator.UnitTests
{
    [TestClass]
    public class TestOneRosterGeneration
    {
        [TestMethod]
        public void TestSchoolCount()
        {
            var oneRoster = new OneRoster(schoolCount: 10);

            Assert.AreEqual(10, oneRoster.Orgs.Where(x => x.OrgType == Models.OrgType.school).Count());
        }

        //TODO: Testing class size where would be great, but there is some random variance in how the number of students per grade

        [TestMethod]
        public void TestClassSizeCount()
        {
            var oneRoster = new OneRoster(classSize: 10);

            var classId = oneRoster.Classes.First().SourcedId;
            Assert.AreEqual(10, oneRoster.Enrollments.Where(x => x.ClassSourcedId == classId && x.RoleType == Models.RoleType.student).Count());
        }

        [TestMethod]
        public void TestStudentIdStart()
        {
            var oneRoster = new OneRoster(studentIdStart: 1000);

            Assert.AreEqual(1000, oneRoster.Students.Min(x => int.Parse(x.Identifier)));
        }

        [TestMethod]
        public void TestStaffIdStart()
        {
            var oneRoster = new OneRoster(staffIdStart: 1000);

            Assert.AreEqual(1000, oneRoster.Staff.Min(x => int.Parse(x.Identifier)));
        }
    }
}
