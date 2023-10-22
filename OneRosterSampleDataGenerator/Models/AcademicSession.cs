using System;

namespace OneRosterSampleDataGenerator.Models;

public class AcademicSession : BaseModel
{
    public DateTime DateLastModified { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime StartDate { get; set; }
    public Guid SourcedId { get; set; }
    public string SchoolYear { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Type => SessionType.ToString();
    public SessionType SessionType { get; set; }
    public AcademicSession? ParentAcademicSession { get; set; }
}
