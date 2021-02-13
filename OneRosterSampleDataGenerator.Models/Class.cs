using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class @Class : BaseModel
    {
        public Guid sourcedId { get; set; }
        public string status => this.Status.ToString();
        public DateTime dateLastModified => CreatedAt;
        public string title { get; set; }
        public string grades { get; set; }
        public Guid courseSourcedId { get; set; }
        public string classCode { get; set; }
        public string classType { get; set; }
        public Guid schoolSourcedId { get; set; }
        public Guid termSourcedid { get; set; }
    }
}
