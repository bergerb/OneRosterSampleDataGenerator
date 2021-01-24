using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace OneRosterSampleDataGenerator.UnitTests
{
    [TestClass]
    public class TestStudents
    {
        [TestMethod]
        public void TestStudentsAvailable()
        {
            var OneRoster = new OneRoster();
            // check for valid grades
            Assert.IsTrue(OneRoster.students.Count() > 0);
        }
    }
}
