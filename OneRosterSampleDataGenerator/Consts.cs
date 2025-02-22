using System.Collections.Generic;

namespace OneRosterSampleDataGenerator;

public static class Consts
{
    // Individual filename constants
    public const string ClassesFile = "classes.csv";
    public const string CoursesFile = "courses.csv";
    public const string DemographicsFile = "demographics.csv";
    public const string EnrollmentsFile = "enrollments.csv";
    public const string ManifestFile = "manifest.csv";
    public const string OrgsFile = "orgs.csv";
    public const string UsersFile = "users.csv";

    public static readonly List<string> OneRosterFiles =
    [
        ClassesFile,
        CoursesFile,
        DemographicsFile,
        EnrollmentsFile,
        ManifestFile,
        OrgsFile,
        UsersFile,
    ];

    // Elementary
    public const string Kintergarden = "KG";
    public const string First = "01";
    public const string Second = "02";
    public const string Third = "03";
    public const string Fourth = "04";
    public const string Fifth = "05";
    // Middle
    public const string Sixth = "06";
    public const string Seventh = "07";
    public const string Eighth = "08";
    // High
    public const string Ninth = "09";
    public const string Tenth = "10";
    public const string Eleventh = "11";
    public const string Twelveth = "12";

    public const string ElementaryName = "Elementary";
    public const string MiddleName = "Middle";
    public const string HighName = "High";

    public readonly static string[] SchoolLevels =
    [
        ElementaryName,
        MiddleName,
        HighName,
    ];

    public readonly static string[] Elementary =
    [
        Kintergarden,
        First,
        Second,
        Third,
        Fourth,
        Fifth,
    ];

    public readonly static string[] Middle =
    [
        Sixth,
        Seventh,
        Eighth,
    ];

    public readonly static string[] High =
    [
        Ninth,
        Tenth,
        Eleventh,
        Twelveth,
    ];

    public static readonly string[] All = [.. Elementary, .. Middle, .. High];
}
