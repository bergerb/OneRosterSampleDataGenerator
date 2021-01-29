using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class AcademicSession
    {
        public Guid sourcedId { get; set; }
        public string status { get; set; }
        public DateTime dateLastModified { get; set; }
        public string title { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string type { get; set; }
        public string schoolYear { get; set; }
    }
}
