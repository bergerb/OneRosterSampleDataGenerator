using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;

namespace Tests;

public class CoursesTest : RosterTest
{
    [Test]
    public void CourseCount_ShouldBeGreaterThanZero_WhenGenerated()
    {
        OneRoster.Courses.Count.ShouldBeGreaterThan(0);
    }
}
