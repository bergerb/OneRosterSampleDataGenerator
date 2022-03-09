using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;
using System.Linq;

namespace Tests
{
    public class TestGrades
    {
        [Test]
        public void TestGradeAvailable()
        {
            var OneRoster = new OneRoster();
            // check for valid grades
            OneRoster.Grades.Count.ShouldBeGreaterThan(0);
        }
    }
}
