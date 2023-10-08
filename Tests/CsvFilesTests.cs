using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;
using System.IO;
using System.IO.Compression;

namespace Tests;

public class CsvFilesTests : RosterTest
{
    [Test]
    public void OutputCSVFiles_ShouldBeEight_WhenGenerated()
    {
        OneRoster.OutputCSVFiles();
        Directory.GetFiles("OneRoster")
            .Length
            .ShouldBe(8);
    }

    [Test]
    public void OutputOneRosterZipFile_ShouldCreateZipFile_WhenGenerated()
    {
        OneRoster.OutputOneRosterZipFile();
        File.Exists("OneRoster.zip")
            .ShouldBeTrue();
    }

    [Test]
    public void OutputOneRosterZipFile_ShouldCreateZipFile_WhenGeneratedWith3Schools()
    {
        var OneRoster = new OneRoster(new() { SchoolCount = 3 });
        OneRoster.OutputOneRosterZipFile();
    }

    [Test]
    public void OutputOneRosterZipFile_ShouldIncludeAllFilesInZipFile_WhenGenerated()
    {
        OneRoster.OutputOneRosterZipFile();
        string zipFile = @"OneRoster.zip";
        using ZipArchive archive = ZipFile.OpenRead(zipFile);
        archive.Entries.Count.ShouldBe(8);
    }

    [Test]
    public void SettingIncremental_ShouldCreatedFiveFiles_WhenGenerated()
    {
        var oneRoster = new OneRoster(new() { IncrementalDaysToCreate = 5, SchoolCount = 3 });

        Directory.GetFiles(".", "*.zip")
            .Length
            .ShouldBe(5);
        Directory.GetFiles(".", "*OneRosterChanges.txt")
            .Length
            .ShouldBe(1);
    }
}
