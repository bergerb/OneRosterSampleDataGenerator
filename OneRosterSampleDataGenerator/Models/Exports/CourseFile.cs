using CsvHelper.Configuration.Attributes;
using OneRosterSampleDataGenerator.Models.Interfaces;
using System;

namespace OneRosterSampleDataGenerator.Models.Exports;

public class CourseFile : IExportable<Course, CourseFile>
{
    [Name("sourcedId")]
    public string SourcedId { get; set; } = null!;
    [Name("status")]
    public string Status { get; set; } = null!;
    [Name("dateLastModified")]
    [Format("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTime DateLastModified { get; set; }
    [Name("schoolYearSourcedId")]
    public string SchoolYearSourcedId { get; set; } = null!;
    [Name("title")]
    public string Title { get; set; } = null!;
    [Name("courseCode")]
    public string CourseCode { get; set; } = null!;
    [Name("grades")]
    public string Grades { get; set; } = null!;
    [Name("orgSourcedId")]
    public string OrgSourcedId { get; set; } = null!;
    [Name("subjects")]
    public string Subjects { get; set; } = null!;
    [Name("subjectCodes")]
    public string SubjectCodes { get; set; } = null!;

    public CourseFile Map(Course item)
    {
        return new()
        {
            SourcedId = item.SourcedId.ToString(),
            Status = item.Status.ToString(),
            DateLastModified = item.DateLastModified,
            SchoolYearSourcedId = item.SchoolYearSourcedId.ToString(),
            Title = item.Title,
            CourseCode = item.CourseCode,
            Grades = item.Grade.Name,
            OrgSourcedId = item.OrgSourcedId.ToString(),
            Subjects = "",
            SubjectCodes = "",
        };
    }
}
