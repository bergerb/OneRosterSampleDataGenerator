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
}
