using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Course : BaseModel
    {
        public Guid sourcedId { get; set; }
        public DateTime dateLastModified => CreatedAt;
        public Guid schoolYearSourcedId { get; set; }
        public string title { get; set; }
        public string courseCode { get; set; }
        public Guid orgSourcedId { get; set; }
        public Grade grade { get; set; }
    }
}
