using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;
using System.IO.Compression;
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

        [Test]
        public void TestOneRosterZipGeneration3Schools()
        {
            var OneRoster = new OneRoster(
                schoolCount: 3);
            OneRoster.OutputOneRosterZipFile();
        }

        [Test]
        public void TestOneRosterZipToEnsureAllZipFiles()
        {
            var OneRoster = new OneRoster();
            OneRoster.OutputOneRosterZipFile();
            string zipFile = @"OneRoster.zip";
            using ZipArchive archive = ZipFile.OpenRead(zipFile);
            archive.Entries.Count().ShouldBe(8);
        }


    }
}
