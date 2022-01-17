using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Org : BaseModel
    {
        public new string Id => this.SourcedId.ToString();
        public Guid SourcedId { get; set; }
        public DateTime DateLastModified => CreatedAt;
        public string Name { get; set; }
        public OrgType OrgType { get; set; }
        public string Type => OrgType.ToString();
        public string Identifier { get; set; }
        public Guid? ParentSourcedId { get; set; }
        public List<Grade> GradesOffer { get; set; } = new List<Grade>();
        public bool isHigh => this.Name.Contains("High");
        public bool isMiddle => this.Name.Contains("Middle");
        public bool isElementary => this.Name.Contains("Elementary");
    }
}
