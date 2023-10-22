using CsvHelper.Configuration.Attributes;
using OneRosterSampleDataGenerator.Models.Interfaces;
using System;

namespace OneRosterSampleDataGenerator.Models.Exports;

public class ClassFile : IExportable<Class, ClassFile>
{
    [Name("sourcedId")]
    public string SourcedId { get; set; } = null!;
    [Name("status")]
    public string Status { get; set; } = null!;
    [Name("dateLastModified")]
    [Format("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTime DateLastModified { get; set; }
    [Name("title")]
    public string Title { get; set; }
    [Name("grades")]
    public string Grades { get; set; } = null!;
    [Name("courseSourcedId")]
    public string CourseSourcedId { get; set; } = null!;
    [Name("classCode")]
    public string ClassCode { get; set; } = null!;
    [Name("classType")]
    public string ClassType { get; set; } = null!;
    [Name("location")]
    public string Location { get; set; } = null!;
    [Name("schoolSourcedId")]
    public string SchoolSourcedId { get; set; } = null!;
    [Name("termSourcedId")]
    public string TermSourcedId { get; set; } = null!;
    [Name("subjects")]
    public string Subjects { get; set; } = null!;
    [Name("SubjectCodes")]
    public string SubjectCodes { get; set; } = null!;
    [Name("periods")]
    public string Periods { get; set; } = null!;

    public ClassFile Map(Class item)
    {
        return new()
        {
            ClassCode = item.ClassCode,
            ClassType = item.ClassType,
            CourseSourcedId = item.CourseSourcedId.ToString(),
            DateLastModified = item.DateLastModified,
            Grades = item.Grades,
            Location = "",
            Periods = "",
            SchoolSourcedId = item.SchoolSourcedId.ToString(),
            SourcedId = item.SourcedId.ToString(),
            Status = item.Status.ToString(),
            SubjectCodes = "",
            Subjects = "",
            TermSourcedId = "",
            Title = item.Title,
        };
    }
}
