using CsvHelper.Configuration.Attributes;
using OneRosterSampleDataGenerator.Models.Interfaces;
using System;

namespace OneRosterSampleDataGenerator.Models.Exports;

public class EnrollmentFile : IExportable<Enrollment, EnrollmentFile>
{
    [Name("sourcedId")]
    public string SourcedId { get; set; } = null!;
    [Name("status")]
    public string Status { get; set; } = null!;
    [Name("dateLastModified")]
    [Format("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTime DateLastModified { get; set; }
    [Name("classSourcedId")]
    public string ClassSourcedId { get; set; } = null!;
    [Name("schoolSourcedId")]
    public string SchoolSourcedId { get; set; } = null!;
    [Name("userSourcedId")]
    public string UserSourcedId { get; set; } = null!;
    [Name("role")]
    public string Role { get; set; } = null!;
    [Name("primary")]
    public string Primary { get; set; } = null!;
    [Name("beginDate")]
    [Format("yyyy-MM-dd")]
    public DateTime? BeginDate { get; set; }
    [Name("endDate")]
    [Format("yyyy-MM-dd")]
    public DateTime? EndDate { get; set; }

    public EnrollmentFile Map(Enrollment item)
    {
        return new()
        {
            ClassSourcedId = item.ClassSourcedId.ToString(),
            DateLastModified = item.DateLastModified,
            Role = item.Role.ToString(),
            SchoolSourcedId = item.SchoolSourcedId.ToString(),
            SourcedId = item.SourcedId.ToString(),
            Status = item.Status.ToString(),
            UserSourcedId = item.UserSourcedId.ToString(),
        };
    }
}
