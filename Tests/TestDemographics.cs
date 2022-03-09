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
            // check for valid grades
            OneRoster.Demographics.Count.ShouldBeGreaterThan(0);
        }
    }
}
