using System;

namespace OneRosterSampleDataGenerator.Models;

public class @Class : BaseModel
{
    public DateTime DateLastModified { get; set; }
    public Guid CourseSourcedId { get; set; }
    public Guid SchoolSourcedId { get; set; }
    public Guid SourcedId { get; set; }
    public Guid TermSourcedid { get; set; }
    public string ClassCode { get; set; } = null!;
    public string ClassType { get; set; } = null!;
    public string Grades { get; set; } = null!;
    public string Title { get; set; } = null!;
}
