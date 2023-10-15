using System;
using System.Collections.Generic;

namespace OneRosterSampleDataGenerator.Models;

public class Student : BaseModel, IUser
{
    public bool EnabledUser { get; set; }
    public DateTime DateLastModified { get; set; }
    public Guid SourcedId { get; set; }
    public Grade Grade { get; set; } = null!;
    public List<Course> Courses { get; set; } = new List<Course>();
    public string CurrentGrade => Grade.Name;
    public string CurrentOrgName => Org.Name;
    public string Email => $"{UserName}@domain.local";
    /// <summary>
    /// Last Name
    /// </summary>
    public string FamilyName { get; set; } = null!;
    /// <summary>
    /// First Name
    /// </summary>
    public string GivenName { get; set; } = null!;
    public string Identifier { get; set; } = null!;
    public string UserName => $"{GivenName[..1]}{FamilyName}{Identifier.Substring(6, 3)}";
    public Org Org { get; set; } = null!;
    public RoleType RoleType { get => RoleType.student; set => throw new NotImplementedException(); }
}
