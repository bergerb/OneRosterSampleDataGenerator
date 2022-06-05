using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Course : BaseModel
    {
        public DateTime DateLastModified => CreatedAt;
        public Guid SchoolYearSourcedId { get; set; }
        public Guid SourcedId { get; set; }
        public Guid OrgSourcedId { get; set; }
        public Grade Grade { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public string Title { get; set; } = null!;
    }
}
