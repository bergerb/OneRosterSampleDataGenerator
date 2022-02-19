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
            OneRoster.OutputCSVFiles();
        }

        [TestMethod]
        public void TestOneRosterZipGeneration()
        {
            var OneRoster = new OneRoster();
            OneRoster.OutputOneRosterZipFile();
        }
    }
}
