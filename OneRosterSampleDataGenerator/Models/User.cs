using OneRosterSampleDataGenerator.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace OneRosterSampleDataGenerator.Models;

public class User : BaseModel, ILeaUser
{
    public bool EnabledUser { get; set; }
    public DateTime DateLastModified { get; set; }
    public Guid SourcedId { get; set; }
    public Grade? Grade { get; set; }
    public List<Course> Courses { get; set; } = [];
    public string? CurrentGrade => $"{this.Grade?.Name}";
    public string CurrentOrgName => this.Org.Name;
    public string Email => $"{this.UserName}@domain.local";
    public string FamilyName { get; set; } = null!;
    public string GivenName { get; set; } = null!;
    public string Identifier { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public Org Org { get; set; } = null!;
    public RoleType RoleType { get; set; }
    public List<Class> Classes { get; set; } = [];

    public void AddClass(Class @class)
    {
        this.Classes.Add(@class);
    }

    public static User Map(ILeaUser user)
    {

        return new()
        {
            EnabledUser = user.EnabledUser,
            DateLastModified = user.DateLastModified,
            SourcedId = user.SourcedId,
            Grade = user.Grade,
            Courses = user.Courses,
            FamilyName = user.FamilyName,
            GivenName = user.GivenName,
            Identifier = user.Identifier,
            UserName = user.UserName,
            Org = user.Org,
            RoleType = user.RoleType,
        };
    }
}
