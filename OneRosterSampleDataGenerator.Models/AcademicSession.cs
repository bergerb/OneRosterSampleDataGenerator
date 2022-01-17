using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class AcademicSession : BaseModel
    {
        public Guid SourcedId { get; set; }
        public DateTime DateLastModified => CreatedAt;
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SessionType SessionType { get; set; }
        public string Type => SessionType.ToString();
        public string SchoolYear { get; set; }
    }
}
