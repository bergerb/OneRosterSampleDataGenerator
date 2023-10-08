using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;

namespace Tests;

public class ClassesTests : RosterTest
{
    [Test]
    public void ClassCount_ShouldBeGreaterThanZero_WhenGenerated()
    {
        OneRoster.Classes.Count.ShouldBeGreaterThan(0);
    }
}
