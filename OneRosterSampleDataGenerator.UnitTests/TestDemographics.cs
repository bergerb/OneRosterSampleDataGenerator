using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace OneRosterSampleDataGenerator.UnitTests
{
    [TestClass]
    public class TestDemographics
    {
        [TestMethod]
        public void TestDemographicsAvailable()
        {
            var OneRoster = new OneRoster();
            // check for valid grades
            Assert.IsTrue(OneRoster.Demographics.Count() > 0);
        }
    }
}
