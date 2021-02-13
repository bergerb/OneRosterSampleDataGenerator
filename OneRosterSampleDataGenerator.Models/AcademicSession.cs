using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class AcademicSession : BaseModel
    {
        public Guid sourcedId { get; set; }
        public string status => this.Status.ToString();
        public DateTime dateLastModified => CreatedAt;
        public string title { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public SessionType sessionType { get; set; }
        public string type => sessionType.ToString();
        public string schoolYear { get; set; }
    }
}
