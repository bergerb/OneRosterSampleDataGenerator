using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class @Class
    {
        public Guid id { get; set; }
        public string status { get; set; }
        public DateTime dateLastModified { get; set; }
        public string grades { get; set; }
        public Guid courseSourcedId { get; set; }
        public string title { get; set; }
        public string classCode { get; set; }
        public Guid schoolSourcedId { get; set; }
        public Guid termSourcedid { get; set; }
        public string classType { get; set; }
    }
}
