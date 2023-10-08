using NUnit.Framework;
using OneRosterSampleDataGenerator;
using OneRosterSampleDataGenerator.Models;
using Shouldly;
using System;
using System.Linq;
using static OneRosterSampleDataGenerator.OneRoster;

namespace Tests;

public class OneRosterGenerationTests
{
    [Test]
    public void OrgCount_ShouldBeEqualToSchoolCount_WhenGeneratedAndSetToValue()
    {
        var oneRoster = new OneRoster(new() { SchoolCount = 22 });

        oneRoster.Orgs.Count(x => x.OrgType == OrgType.school).ShouldBe(22);
    }


    //TODO: Testing class size where would be great, but there is some random variance in how the number of students per grade
    [Test]
    public void ClassSize_ShouldBeTen_WhenGeneratedAndClassSizeSetToTen()
    {
        var oneRoster = new OneRoster(new() { ClassSize = 10 });

        var classId = oneRoster.Classes.First().SourcedId;
        oneRoster.Enrollments.Count(x => x.ClassSourcedId == classId && x.RoleType == RoleType.student).ShouldBe(10);
    }

    [Test]
    public void StudentIdStart_ShouldMatchStartId_WhenGeneratedAndStartIdSet()
    {
        var oneRoster = new OneRoster(new() { StudentIdStart = 1000 });

        oneRoster.Students.Min(x => int.Parse(x.Identifier)).ShouldBe(1000);
    }

    [Test]
    public void StaffIdStart_ShouldMatchStartId_WhenGeneratedAndStartIdSet()
    {
        var oneRoster = new OneRoster(new Args { StaffIdStart = 1000 });

        oneRoster.Staff.Min(x => int.Parse(x.Identifier)).ShouldBe(1000);
    }

    [Test]
    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    public void ArgumentException_ShouldBeThrown_WhenInvalidSchoolCount(int schoolCount)
    {
        Should.Throw<ArgumentException>(() =>
        {
            var oneRoster = new OneRoster(new() { SchoolCount = schoolCount });
        });
    }

    [Test]
    public void EachSchoolLevel_ShouldBeSet_WhenGeneratedThreeSchools()
    {
        var oneRoster = new OneRoster(new() { SchoolCount = 3 });

        var schools = oneRoster.Orgs.Where(x => x.OrgType == OrgType.school);

        schools.Where(x => x.IsElementary).Count().ShouldBe(1);
        schools.Where(x => x.IsMiddle).Count().ShouldBe(1);
        schools.Where(x => x.IsHigh).Count().ShouldBe(1);
    }
}
