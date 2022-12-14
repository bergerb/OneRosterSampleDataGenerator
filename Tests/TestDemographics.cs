using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;
using System.Linq;

namespace Tests
{
    public class TestDemographics
    {
        [Test]
        public void TestDemographicsAvailable()
        {
            var OneRoster = new OneRoster();

            OneRoster.Demographics.Count.ShouldBeGreaterThan(0);
        }

        [Test]
        public void TestDemographicsShouldHaveABirthDateInEachMonth()
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
}
