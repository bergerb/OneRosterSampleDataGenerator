using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class Demographics(DateTime createdAt, List<User> students) : Generator<Demographic>(createdAt)
{
    private static readonly Random _random = new();

    public List<User> Students { get; set; } = students;

    public override List<Demographic> Generate()
    {
        this.Items = this.CreateDemographics().ToList();

        return this.Items;
    }

    public Demographic CreateDemographic(Guid sourcedId, User student)
    {
        var demographic = new Demographic()
        {
            BirthDate = GetBirthday(student, _random),
            CityOfBirth = "",
            CountryOfBirthCode = "",
            DateLastModified = this.CreatedAt,
            PublicSchoolResidenceStatus = "",
            Sex = _random.Next(0, 1) == 0 ? "female" : "male",
            SourcedId = sourcedId,
            StateOfBirthAbbreviation = "",
            Status = StatusType.active,
        };

        return demographic;
    }

    private IEnumerable<Demographic> CreateDemographics()
    {
        foreach (User student in this.Students)
        {
            yield return this.CreateDemographic(student.SourcedId, student);
        }

    }

    private static DateTime GetBirthday(User student, Random rnd)
    {
        return DateTime.Parse($"7/1/{int.Parse(Utility.GetCurrentSchoolYear()) - (4 + student.Grade?.Id)}").AddDays(rnd.Next(0, 365));
    }
}
