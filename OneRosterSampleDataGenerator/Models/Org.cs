using System;
using System.Collections.Generic;

namespace OneRosterSampleDataGenerator.Models;

public class Org : BaseModel
{
    public bool IsElementary => this.Name.Contains(Consts.ElementaryName);
    public bool IsHigh => this.Name.Contains(Consts.HighName);
    public bool IsMiddle => this.Name.Contains(Consts.MiddleName);
    public DateTime DateLastModified { get; set; }
    public Guid SourcedId { get; set; }
    public Guid? ParentSourcedId { get; set; }
    public List<Grade> GradesOffer { get; set; } = [];
    public new string Id => this.SourcedId.ToString();
    public OrgType OrgType { get; set; }
    public string Name { get; set; } = null!;
    public string Type => this.OrgType.ToString();
    public string Identifier { get; set; } = null!;
}
