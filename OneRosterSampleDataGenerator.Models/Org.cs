using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Org : BaseModel
    {
        public bool IsElementary => Name.Contains("Elementary");
        public bool IsHigh => Name.Contains("High");
        public bool IsMiddle => Name.Contains("Middle");
        public DateTime DateLastModified => CreatedAt;
        public Guid SourcedId { get; set; }
        public Guid? ParentSourcedId { get; set; }
        public List<Grade> GradesOffer { get; set; } = new List<Grade>();
        public new string Id => this.SourcedId.ToString();
        public OrgType OrgType { get; set; }
        public string Name { get; set; } = null!;
        public string Type => OrgType.ToString();
        public string Identifier { get; set; } = null!;
    }
}
