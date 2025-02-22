using Bogus;
using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class Orgs(
    DateTime createdAt,
    Org parentOrg,
    int schoolCount,
    List<Grade> grades) : Generator<Org>(createdAt)
{
    private readonly Faker _faker = new("en");
    private static readonly Random _random = new();
    public Org parentOrg { get; set; } = parentOrg;
    public int schoolCount { get; set; } = schoolCount;
    public List<Grade> grades { get; set; } = grades;

    public override List<Org> Generate()
    {
        this.parentOrg.DateLastModified = this.CreatedAt;

        this.Items = this.CreateOrgs().ToList();

        this.AddItem(this.parentOrg);

        return [.. this.Items];
    }

    private IEnumerable<Org> CreateOrgs()
    {
        string[] schoolTypes = ["Elementary School", "Elementary School", "Middle School", "Middle School", "High School"];

        for (int count = 0; count < this.schoolCount; count++)
        {
            string schoolNamePrefix = _faker.Address.City();
            var paddedOrgNum = ("0000" + count.ToString());
            var identifier = paddedOrgNum.Substring(paddedOrgNum.Length - 4, 4);
            var schoolName = this.schoolCount != 3 ?
                $"{schoolNamePrefix} {schoolTypes[_random.Next(schoolTypes.Length)]}" :
                $"{schoolNamePrefix} {Consts.SchoolLevels[count]}";

            yield return
                CreateSchool(
                    identifier,
                    schoolName,
                    this.CreatedAt,
                    this.parentOrg.SourcedId,
                    this.grades
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
        if (newOrg.Name.Contains(Consts.ElementaryName))
        {
            newOrg.GradesOffer = grades.Where(e => Consts.Elementary.Contains(e.Name)).ToList();
        }
        if (newOrg.Name.Contains(Consts.MiddleName))
        {
            newOrg.GradesOffer = grades.Where(e => Consts.Middle.Contains(e.Name)).ToList();
        }
        if (newOrg.Name.Contains(Consts.HighName))
        {
            newOrg.GradesOffer = grades.Where(e => Consts.High.Contains(e.Name)).ToList();
        }
        return newOrg;
    }
}
