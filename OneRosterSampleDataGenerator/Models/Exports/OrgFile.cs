using CsvHelper.Configuration.Attributes;
using OneRosterSampleDataGenerator.Models.Interfaces;
using System;

namespace OneRosterSampleDataGenerator.Models.Exports;

public class OrgFile : IExportable<Org, OrgFile>
{
    [Name("sourcedId")]
    public string SourcedId { get; set; } = null!;
    [Name("status")]
    public string Status { get; set; } = null!;
    [Name("dateLastModified")]
    [Format("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTime DateLastModified { get; set; }
    [Name("name")]
    public string Name { get; set; } = null!;
    [Name("type")]
    public string Type { get; set; } = null!;
    [Name("identifier")]
    public string Identifier { get; set; } = null!;
    [Name("parentSourcedId")]
    public string ParentSourcedId { get; set; } = null!;

    public OrgFile Map(Org item)
    {
        return new()
        {
            SourcedId = item.SourcedId.ToString(),
            Status = item.Status.ToString(),
            DateLastModified = item.DateLastModified,
            Name = item.Name,
            Type = item.Type,
            Identifier = item.Identifier,
            ParentSourcedId = item.ParentSourcedId.ToString(),
        };
    }
}
