using OneRosterSampleDataGenerator.Helpers;
using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneRosterSampleDataGenerator.Models;

public class Orgs : Generator<Org>
{
    public Orgs(
        DateTime createdAt,
        Org parentOrg,
        int schoolCount,
        List<Grade> grades) : base(createdAt)
    {
        this.parentOrg = parentOrg;
        this.schoolCount = schoolCount;
        this.grades = grades;
    }

    public Org parentOrg { get; set; }
    public int schoolCount { get; set; }
    public List<Grade> grades { get; set; }

    public override List<Org> Generate()
    {
        parentOrg.DateLastModified = CreatedAt;

        Items = CreateOrgs().ToList();

        AddItem(parentOrg);

        return Items.ToList();
    }

    private IEnumerable<Org> CreateOrgs()
    {
        string[] schools = Encoding.
          ASCII.
          GetString(Utility.StringToMemoryStream(Properties.Resources.orgs).ToArray()).
          Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        var maxSchools = schools.Length - 1;
        var rnd = new Random();

        var randomSeq = Enumerable.Range(1, maxSchools).OrderBy(r => rnd.NextDouble()).Take(schoolCount).ToList();
        string[] schoolTypes = { "Elementary School", "Elementary School", "Middle School", "Middle School", "High School" };

        for (int count = 0; count < randomSeq.Count; count++)
        {
            string line = schools[randomSeq[count]];
            var paddedOrgNum = ("0000" + randomSeq[count].ToString());
            var identifier = paddedOrgNum.Substring(paddedOrgNum.Length - 4, 4);
            var schoolName = schoolCount != 3 ?
                $"{line} {schoolTypes[rnd.Next(schoolTypes.Length)]}" :
                $"{line} {GradeHelper.SchoolLevels[count]}";

            yield return
                CreateSchool(
                    identifier,
                    schoolName,
                    CreatedAt,
                    parentOrg.SourcedId,
                    grades
                );
        }
    }
    private static Org CreateSchool(string identifier, string name, DateTime dateLastModified, Guid parentOrgId, List<Grade> grades)
    {
        Org newOrg = new()
        {
            DateLastModified = dateLastModified,
            Identifier = identifier,
            Name = name,
            OrgType = OrgType.school,
            ParentSourcedId = parentOrgId,
            SourcedId = Guid.NewGuid(),
        };
        if (newOrg.Name.Contains("Elementary"))
        {
            newOrg.GradesOffer = grades.Where(e => GradeHelper.Elementary.Contains(e.Name)).ToList();
        }
        if (newOrg.Name.Contains("Middle"))
        {
            newOrg.GradesOffer = grades.Where(e => GradeHelper.Middle.Contains(e.Name)).ToList();
        }
        if (newOrg.Name.Contains("High"))
        {
            newOrg.GradesOffer = grades.Where(e => GradeHelper.High.Contains(e.Name)).ToList();
        }
        return newOrg;
    }
}
