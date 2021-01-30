using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace OneRosterSampleDataGenerator.UnitTests
{
    [TestClass]
    public class TestClasses
    {
        [TestMethod]
        public void TestClassesAvailable()
        {
            var OneRoster = new OneRoster();
            // check for valid grades
            Assert.IsTrue(OneRoster.classes.Count() > 0);
        }
    }
}
