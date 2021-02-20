using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
namespace OneRosterSampleDataGenerator.UnitTests
{
    [TestClass]
    public class TestCSVFiles
    {
        [TestMethod]
        public void TestCSVGeneration()
        {
            var OneRoster = new OneRoster();
            OneRoster.outputCSVFiles();
        }
    }
}
