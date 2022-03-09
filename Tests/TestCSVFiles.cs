using NUnit.Framework;
using OneRosterSampleDataGenerator;
using System.Linq;

namespace Tests
{
    public class TestCSVFiles
    {
        [Test]
        public void TestCSVGeneration()
        {
            var OneRoster = new OneRoster();
            OneRoster.OutputCSVFiles();
        }

        [Test]
        public void TestOneRosterZipGeneration()
        {
            var OneRoster = new OneRoster();
            OneRoster.OutputOneRosterZipFile();
        }
    }
}
