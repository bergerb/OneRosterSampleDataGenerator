using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;
using System.Linq;

namespace Tests;

public class DemographicsTests : RosterTest
{
    [Test]
    public void DemographicsCount_ShouldBeGreaterThanZero_WhenGenerated()
    {
        OneRoster.Demographics.Count.ShouldBeGreaterThan(0);
    }

    /// <summary>
    /// Note this test be generate flaky data due to the nature of randomness
    /// </summary>
    [Test]
    public void DemographicsBirthdates_ShouldHaveOneInEachMonth_WhenGenerated()
    {
        OneRoster oneRoster = new();

        foreach (var month in Enumerable.Range(1, 12))
        {
            oneRoster.Demographics
                .Any(x => x.BirthDate.Month == month)
                .ShouldBeTrue();
        }
    }
}
