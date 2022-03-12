using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class @Class : BaseModel
    {
        public Guid SourcedId { get; set; }
        public DateTime DateLastModified => CreatedAt;
        public string Title { get; set; } = null!;
        public string Grades { get; set; } = null!;
        public Guid CourseSourcedId { get; set; }
        public string ClassCode { get; set; } = null!;
        public string ClassType { get; set; } = null!;
        public Guid SchoolSourcedId { get; set; }
        public Guid TermSourcedid { get; set; }
    }
}
