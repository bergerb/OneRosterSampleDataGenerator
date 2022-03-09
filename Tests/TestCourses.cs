using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;
using System.Linq;

namespace Tests
{
    public class TestCourses
    {
        [Test]
        public void TestCoursesAvailable()
        {
            var OneRoster = new OneRoster();
            // check for valid grades
            OneRoster.Courses.Count.ShouldBeGreaterThan(0);
        }
    }
}
