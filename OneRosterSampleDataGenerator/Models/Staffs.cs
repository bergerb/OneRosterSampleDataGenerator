using Bogus;
using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public record Staffs(DateTime createdAt, List<Org> orgs) : Generator<Staff>
{
    readonly Faker faker = new("en");

    public override List<Staff> Generate()
    {
        return Items.ToList();
    }

    public void GenerateAdministration()
    {
        foreach (Org org in orgs.Where(e => e.OrgType == OrgType.school))
        {
            for (int i = 0; i < (org.IsHigh ? 3 : org.IsMiddle ? 2 : org.IsElementary ? 1 : 1); i++)
            {
                CreateStaff(org, RoleType.administrator);
            }
        }
    }
    public Staff CreateStaff(Org org = null, RoleType roleType = RoleType.teacher)
    {
        var staffid = "00000000" + RunningId.ToString();

        Staff newStaff = new()
        {
            SourcedId = Guid.NewGuid(),
            DateLastModified = createdAt,
            Identifier = staffid.Substring(staffid.Length - 8, 8),
            EnabledUser = true,
            GivenName = faker.Name.FirstName(),
            FamilyName = faker.Name.LastName(),
            RoleType = roleType,
            Org = org
        };

        newStaff.UserName = Utility.CreateTeacherUserName(Items, newStaff.GivenName, newStaff.FamilyName);

        RunningId++;

        AddItem(newStaff);

        return newStaff;
    }

}
