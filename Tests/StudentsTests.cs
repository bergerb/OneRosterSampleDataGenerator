using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;
using System.Linq;

namespace Tests;

public class StudentsTests : RosterTest
{
    [Test]
    public void StudentCount_ShouldBeGreaterThanZero_WhenGenerated()
    {
        // check for students
        OneRoster.Students.Count.ShouldBeGreaterThan(0);
    }

    [Test]
    public void StudentEmail_ShouldHaveEmailAddress_WhenGenerated()
    {
        OneRoster.Students
            .ShouldAllBe(x => x.Email.Contains('@'));
    }

    [Test]
    public void StudentIds_ShouldAllBeUnique_WhenGenerated()
    {
        var students = (from s in OneRoster.Students
                        group s.Identifier by s.Identifier into dupes
                        where dupes.Count() > 1
                        select new { identifier = dupes.Key, count = dupes.Count() })
                       .ToList();

        students.Count.ShouldBe(0);
    }

    [Test]
    public void Students_ShouldAllBeInAnOrg_WhenGenerated()
    {
        OneRoster.Students
            .Count(e => e.Org == null)
            .ShouldBe(0);
    }
}
