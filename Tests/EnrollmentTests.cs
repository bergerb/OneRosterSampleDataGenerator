using NUnit.Framework;
using OneRosterSampleDataGenerator;
using OneRosterSampleDataGenerator.Models;
using Shouldly;
using System.Linq;

namespace Tests;

public class EnrollmentTests : RosterTest
{
    [Test]
    public void EnrollmentCountForTeachers_ShouldBeGreaterThanZero_WhenGenerated()
    {
        OneRoster.Enrollments.Count(e => e.RoleType == RoleType.teacher).ShouldBeGreaterThan(0);
    }

    [Test]
    public void EnrollmentCountForStudents_ShouldBeGreaterThanZero_WhenGenerated()
    {
        OneRoster.Enrollments.Count(e => e.RoleType == RoleType.student).ShouldBeGreaterThan(0);
    }
}
