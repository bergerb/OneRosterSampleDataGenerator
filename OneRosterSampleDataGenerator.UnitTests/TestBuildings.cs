using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace OneRosterSampleDataGenerator.UnitTests
{
    [TestClass]
    public class TestBuildings
    {
        [TestMethod]
        public void TestBuildingsAvailable()
        {
            var OneRoster = new OneRoster();
            // check for valid grades
            Assert.IsTrue(OneRoster.buildings.Count() > 0);
        }
    }
}
