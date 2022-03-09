using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;
using System.Linq;

namespace Tests
{
    public class TestClasses
    {
        [Test]
        public void TestClassesAvailable()
        {
            var OneRoster = new OneRoster();
            // check for valid grades
            OneRoster.Classes.Count.ShouldBeGreaterThan(0);
        }
    }
}
