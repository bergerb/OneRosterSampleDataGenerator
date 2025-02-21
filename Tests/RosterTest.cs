using NUnit.Framework;
using OneRosterSampleDataGenerator;
using System;
using System.IO;

namespace Tests;

[TestFixture]
public class RosterTest
{
    public OneRoster OneRoster;

    [SetUp]
    public void SetUp()
    {
        string[] zipFiles = Directory.GetFiles(".", "*.zip");

        foreach (string zipFile in zipFiles)
        {
            File.Delete(zipFile);
            Console.WriteLine($"Deleted: {zipFile}");
        }

        OneRoster = new OneRoster();
    }
}