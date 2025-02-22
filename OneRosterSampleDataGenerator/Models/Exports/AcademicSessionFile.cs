using CsvHelper.Configuration.Attributes;
using OneRosterSampleDataGenerator.Models.Interfaces;
using System;

namespace OneRosterSampleDataGenerator.Models.Exports;

public class AcademicSessionFile : IExportable<AcademicSession, AcademicSessionFile>
{
    [Name("sourcedId")]
    public string SourcedId { get; set; } = null!;
    [Name("status")]
    public string Status { get; set; } = null!;
    [Name("dateLastModified")]
    [Format("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTime DateLastModified { get; set; }
    [Name("title")]
    public string Title { get; set; } = null!;
    [Name("type")]
    public string Type { get; set; } = null!;
    [Name("startDate")]
    [Format("yyyy-MM-dd")]
    public DateTime StartDate { get; set; }
    [Name("endDate")]
    [Format("yyyy-MM-dd")]
    public DateTime EndDate { get; set; }
    [Name("parentSourcedId")]
    public string ParentSourcedId { get; set; } = null!;
    [Name("schoolYear")]
    public string SchoolYear { get; set; } = null!;

    public AcademicSessionFile Map(AcademicSession item)
    {
        return new()
        {
            DateLastModified = item.DateLastModified,
            EndDate = item.EndDate,
            StartDate = item.StartDate,
            ParentSourcedId = item.ParentAcademicSession?.SourcedId.ToString() ?? "",
            SchoolYear = item.SchoolYear,
            SourcedId = item.SourcedId.ToString(),
            Status = item.Status.ToString(),
            Title = item.Title,
            Type = item.Type
        };


    }
}
