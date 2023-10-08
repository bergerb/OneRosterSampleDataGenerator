using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;

namespace Tests;

public class GradesTests : RosterTest
{
    [Test]
    public void GradesCount_ShouldBeGreaterThanZero_WhenGenerated()
    {
        OneRoster.Grades.Count.ShouldBeGreaterThan(0);
    }
}
