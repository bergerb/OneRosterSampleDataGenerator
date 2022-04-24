using NUnit.Framework;
using OneRosterSampleDataGenerator;
using OneRosterSampleDataGenerator.Models;
using Shouldly;
using System;
using System.Linq;

namespace Tests
{
    public class TestOneRosterGeneration
    {
        [Test]
        public void TestSchoolCount()
        {
            var oneRoster = new OneRoster(schoolCount: 10);

            oneRoster.Orgs.Count(x => x.OrgType == OrgType.school).ShouldBe(10);
        }

        //TODO: Testing class size where would be great, but there is some random variance in how the number of students per grade

        [Test]
        public void TestClassSizeCount()
        {
            var oneRoster = new OneRoster(classSize: 10);

            var classId = oneRoster.Classes.First().SourcedId;
            oneRoster.Enrollments.Count(x => x.ClassSourcedId == classId && x.RoleType == RoleType.student).ShouldBe(10);
        }

        [Test]
        public void TestStudentIdStart()
        {
            var oneRoster = new OneRoster(studentIdStart: 1000);

            oneRoster.Students.Min(x => int.Parse(x.Identifier)).ShouldBe(1000);
        }

        [Test]
        public void TestStaffIdStart()
        {
            var oneRoster = new OneRoster(staffIdStart: 1000);

            oneRoster.Staff.Min(x => int.Parse(x.Identifier)).ShouldBe(1000);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void TestLessThan3SchoolsThrowsException(int schoolCount)
        {
            Should.Throw<ArgumentException>(() =>
            {
                var oneRoster = new OneRoster(schoolCount: schoolCount);
            });
        }

        [Test]
        public void Test3SchoolsShouldReturnOneOfEachSchoolType()
        {
            var oneRoster = new OneRoster(schoolCount: 3);

            var schools = oneRoster.Orgs.Where(x => x.OrgType == OrgType.school);

            schools.Where(x => x.isElementary).Count().ShouldBe(1);
            schools.Where(x => x.isMiddle).Count().ShouldBe(1);
            schools.Where(x => x.isHigh).Count().ShouldBe(1);
        }
    }
}
