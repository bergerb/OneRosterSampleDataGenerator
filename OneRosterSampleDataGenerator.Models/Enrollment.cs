using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Enrollment
    {
        public Guid id { get; set; }
        public string status { get; set; }
        public DateTime dateLastModified { get; set; }
        public Guid classSourcedId { get; set; }
        public Guid courseSourcedId { get; set; }
        public Guid schoolSourcedId { get; set; }
        public Guid userSourcedId { get; set; }
        public string role { get; set; }

    }
}
