using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public record Demographics(
    DateTime createdAt,
    List<User> students) : Generator<Demographic>
{
    public override List<Demographic> Generate()
    {
        Items = CreateDemographics().ToList();

        return Items;
    }

    private IEnumerable<Demographic> CreateDemographics()
    {
        foreach (User student in students)
        {
            var rnd = new Random();

            var demographic = new Demographic()
            {
                DateLastModified = createdAt,
                SourcedId = student.SourcedId,
                Status = StatusType.active,
                BirthDate = DateTime.Parse($"7/1/{int.Parse(Utility.GetCurrentSchoolYear()) - (4 + student.Grade.Id)}")
                                .AddDays(rnd.Next(0, 365)),
                Sex = rnd.Next(0, 1) == 0 ? "female" : "male",
                CountryOfBirthCode = "",
                StateOfBirthAbbreviation = "",
                CityOfBirth = "",
                PublicSchoolResidenceStatus = ""
            };

            yield return demographic;
        }
    }
}
