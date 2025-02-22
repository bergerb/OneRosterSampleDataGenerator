using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class Grades(DateTime createdAt) : Generator<Grade>(createdAt)
{
    public override List<Grade> Generate()
    {
        this.Items = CreateGrades().ToList();

        return this.Items;
    }

    public static IEnumerable<Grade> CreateGrades()
    {
        return Consts.All
            .Select((name, index) => new Grade
            {
                Id = index + 1,
                Name = name
            });
    }
}
