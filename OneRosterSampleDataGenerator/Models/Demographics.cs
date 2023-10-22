using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class Demographics : Generator<Demographic>
{
    public Demographics(DateTime createdAt, List<User> students)
        : base(createdAt)
    {
        Students = students;
    }

    public List<User> Students { get; set; }

    public override List<Demographic> Generate()
    {
        Items = CreateDemographics().ToList();

        return Items;
    }

    private IEnumerable<Demographic> CreateDemographics()
    {
        foreach (User student in Students)
        {
            var rnd = new Random();

            var demographic = new Demographic()
            {
                BirthDate = GetBirthday(student, rnd),
                CityOfBirth = "",
                CountryOfBirthCode = "",
                DateLastModified = CreatedAt,
                PublicSchoolResidenceStatus = "",
                Sex = rnd.Next(0, 1) == 0 ? "female" : "male",
                SourcedId = student.SourcedId,
                StateOfBirthAbbreviation = "",
                Status = StatusType.active,
            };

            yield return demographic;
        }

        static DateTime GetBirthday(User student, Random rnd)
        {
            return DateTime.Parse($"7/1/{int.Parse(Utility.GetCurrentSchoolYear()) - (4 + student.Grade.Id)}").AddDays(rnd.Next(0, 365));
        }
    }
}
