using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Course
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public string courseCode { get; set; }
        public Guid orgSourcedId { get; set; }
        public Guid academicSessionId { get; set; }
        public Grade grade { get; set; }
    }
}
