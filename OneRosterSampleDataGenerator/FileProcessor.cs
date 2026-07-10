using CsvHelper;
using CsvHelper.Configuration;
using OneRosterSampleDataGenerator.Helpers;
using OneRosterSampleDataGenerator.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace OneRosterSampleDataGenerator;

internal record FileProcessor(StatusChangeBuilder statusChangeBuilder)
{
    public void ProcessFile<T1, T2>(IEnumerable<T1> records)
        where T1 : class
        where T2 : class, IExportable<T1, T2>, new()
    {
        var export = new T2();

        var filePath = Path.Combine(OneRoster.OutputDirectory, T2.FileName);

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
