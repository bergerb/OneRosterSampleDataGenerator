using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;

namespace Tests;

public class OrgsTests : RosterTest
{
    [Test]
    public void OrgsCount_ShouldBeGreaterThanZero_WhenGenerated()
    {
        OneRoster.Orgs.Count.ShouldBeGreaterThan(0);
    }
}
