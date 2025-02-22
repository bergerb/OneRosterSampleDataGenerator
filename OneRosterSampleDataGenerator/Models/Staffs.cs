using Bogus;
using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class Staffs : Generator<User>
{
    readonly Faker faker = new("en");

    public Staffs(DateTime createdAt, List<Org> orgs)
        : base(createdAt)
    {
        this.CreatedAt = createdAt;
        this.Orgs = orgs;
    }

    public List<Org> Orgs { get; set; }

    public override List<User> Generate()
    {
        return [.. this.Items];
    }

    public void GenerateAdministration()
    {
        foreach (Org org in this.Orgs.Where(e => e.OrgType == OrgType.school))
        {
            for (int i = 0; i < (org.IsHigh ? 3 : org.IsMiddle ? 2 : org.IsElementary ? 1 : 1); i++)
            {
                this.CreateStaff(org, RoleType.administrator);
            }
        }
    }

    public User CreateStaff(Org org, RoleType roleType = RoleType.teacher)
    {
        var staffid = "00000000" + this.RunningId.ToString();

        User newStaff = new()
        {
            DateLastModified = this.CreatedAt,
            EnabledUser = true,
            FamilyName = faker.Name.LastName(),
            GivenName = faker.Name.FirstName(),
            Identifier = staffid.Substring(staffid.Length - 8, 8),
            Org = org,
            RoleType = roleType,
            SourcedId = Guid.NewGuid(),
        };

        newStaff.UserName = Utility.CreateTeacherUserName(Items, newStaff.GivenName, newStaff.FamilyName);

        this.RunningId++;

        this.AddItem(newStaff);

        return newStaff;
    }

}
