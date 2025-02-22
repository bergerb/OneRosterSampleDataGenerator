using CsvHelper;
using CsvHelper.Configuration;
using OneRosterSampleDataGenerator.Helpers;
using OneRosterSampleDataGenerator.Models;
using OneRosterSampleDataGenerator.Models.Exports;
using OneRosterSampleDataGenerator.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace OneRosterSampleDataGenerator;

internal record FileProcessor(StatusChangeBuilder statusChangeBuilder)
{
    public void ProcessFile<T1, T2>(
        IEnumerable<T1> records,
        string filePath)
    where T1 : class
    where T2 : class, new()
    {
        if (filePath is null)
        {
            throw new ArgumentException("Invalid File Path", nameof(filePath));
        }

        var export = GetExportable<T1, T2>();

        List<T2> exports = ParseRecords(export, records).ToList();

        using var writer = new StreamWriter(filePath);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            ShouldQuote = args => true,
        };
        using var csv = new CsvWriter(writer, config);

        csv.WriteRecords(exports);

        this.statusChangeBuilder.AddEvent(StatusChangeBuilder.EventAction.Created, StatusChangeBuilder.Type.File, typeof(T2).Name);

    }

    private static IExportable<T1, T2> GetExportable<T1, T2>()
        where T1 : class
        where T2 : class, new()
    {
        if (typeof(T1).Name is null)
            throw new ArgumentException("Null type.");

        return typeof(T1).Name switch
        {
            nameof(AcademicSession) => (IExportable<T1, T2>)new AcademicSessionFile(),
            nameof(Class) => (IExportable<T1, T2>)new ClassFile(),
            nameof(Course) => (IExportable<T1, T2>)new CourseFile(),
            nameof(Demographic) => (IExportable<T1, T2>)new DemographicFile(),
            nameof(Enrollment) => (IExportable<T1, T2>)new EnrollmentFile(),
            nameof(Manifest) => (IExportable<T1, T2>)new ManifestFile(),
            nameof(Org) => (IExportable<T1, T2>)new OrgFile(),
            nameof(User) => (IExportable<T1, T2>)new UserFile(),
            _ => throw new ArgumentException("Invalid Type", typeof(T1).Name),
        };
    }

    private static IEnumerable<T2> ParseRecords<T1, T2>(IExportable<T1, T2> export, IEnumerable<T1> records)
        where T1 : class
        where T2 : class, new()
    {
        foreach (var record in records)
        {
            yield return export.Map(record);
        }
    }
}
