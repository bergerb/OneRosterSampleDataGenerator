using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;
using System.Linq;

namespace Tests
{
    public class TestOrgs
    {
        [Test]
        public void TestOrgsAvailable()
        {
            var OneRoster = new OneRoster();
            OneRoster.Orgs.Count.ShouldBeGreaterThan(0);
        }
    }
}
