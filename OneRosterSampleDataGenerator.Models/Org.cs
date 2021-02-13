using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Org : BaseModel
    {
        public Guid sourcedId { get; set; }
        public string status => this.Status.ToString();
        public DateTime dateLastModified => CreatedAt;
        public string name { get; set; }
        public OrgType orgType { get; set; }
        public string type => orgType.ToString();
        public string identifier { get; set; }
        public Guid? parentSourcedId { get; set; }
        public List<Grade> gradesOffer { get; set; }
    }
}
