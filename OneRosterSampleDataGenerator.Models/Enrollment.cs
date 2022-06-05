using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Enrollment : BaseModel
    {
        public DateTime DateLastModified => CreatedAt;
        public Guid SourcedId { get; set; }
        public Guid ClassSourcedId { get; set; }
        public Guid CourseSourcedId { get; set; }
        public Guid SchoolSourcedId { get; set; }
        public Guid UserSourcedId { get; set; }
        public RoleType RoleType { get; set; }
        public string Role => RoleType.ToString();

    }
}
