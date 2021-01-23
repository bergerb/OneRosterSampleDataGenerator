using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace OneRosterSampleDataGenerator.UnitTests
{
    [TestClass]
    public class TestGrades
    {
        [TestMethod]
        public void TestGradeAvailable()
        {
            var OneRoster = new OneRoster();
            // check for valid grades
            Assert.IsTrue(OneRoster.grades.Count() > 0);
        }
    }
}
