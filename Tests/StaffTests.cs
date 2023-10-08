using NUnit.Framework;
using OneRosterSampleDataGenerator;
using OneRosterSampleDataGenerator.Models;
using Shouldly;
using System.Linq;

namespace Tests;

public class StaffTests : RosterTest
{
    [Test]
    public void CreateStaff_CreatesNewStaff()
    {
        Staff newTeacher = OneRoster.CreateStaff();

        newTeacher.ShouldNotBeNull();
        newTeacher.RoleType.ShouldBe(RoleType.teacher);
        newTeacher.Org.ShouldBeNull();
    }

    [Test]
    public void CreateStaff_CreatesNewStaffAtOrg_WhenOrgAdded()
    {
        Staff newTeacher = OneRoster.CreateStaff(OneRoster.Orgs.First());

        newTeacher.ShouldNotBeNull();
        newTeacher.RoleType.ShouldBe(RoleType.teacher);
        newTeacher.Org.ShouldNotBeNull();
    }

    [Test]
    public void Staff_ShouldHaveAdmins_WhenGenerated()
    {
        OneRoster.Staff
            .Any(x => x.RoleType == RoleType.administrator)
            .ShouldBeTrue();
    }

    [Test]
    public void StaffValidity_ShouldBeUnique_WhenGenerated()
    {
        // check for teacher email uniqueness in enrollments
        OneRoster.Staff.GroupBy(x => x.Email)
                    .Where(x => x.Count() > 1)
                    .Any()
                    .ShouldBeFalse();
    }

    [Test]
    public void StaffValidity_ShouldBeOnePerBuilding_WhenGenerated()
    {
        foreach (Org org in OneRoster.Orgs.Where(x => x.OrgType == OrgType.school))
        {
            var staffAdminCount = OneRoster.Staff
                  .Where(x => x.Org.Id == org.Id && x.RoleType == RoleType.administrator).Count();
            Assert.IsTrue(staffAdminCount > 0);
            // High
            if (org.IsHigh)
            {
                staffAdminCount.ShouldBe(3);
            }
            // Middle
            if (org.IsMiddle)
            {
                staffAdminCount.ShouldBe(2);
            }
            // Elementary
            if (org.IsElementary)
            {
                staffAdminCount.ShouldBe(1);
            }
        }
    }
}
