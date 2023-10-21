using OneRosterSampleDataGenerator.Helpers;
using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneRosterSampleDataGenerator.Models;

public record Orgs(
    DateTime createdAt,
    int schoolCount,
    Org parentOrg,
    List<Grade> grades) : Generator<Org>
{
    public override List<Org> Generate()
    {
        parentOrg.DateLastModified = createdAt;

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
                OrgHelper.CreateSchool(
                    identifier,
                    schoolName,
                    createdAt,
                    parentOrg.SourcedId,
                    grades
                );
        }
    }
}
