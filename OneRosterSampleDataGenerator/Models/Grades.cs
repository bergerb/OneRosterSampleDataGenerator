using OneRosterSampleDataGenerator.Models.Base;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public record Grades : Generator<Grade>
{
    public override List<Grade> Generate()
    {
        Items = CreateGrades().ToList();

        return Items;
    }

    private static IEnumerable<Grade> CreateGrades()
    {
        using var reader = new StreamReader(Utility.StringToMemoryStream(Properties.Resources.grades));

        int gradeId = 1;
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line.Split(',');
            Grade newGrade = new()
            {
                Id = gradeId,
                Name = values[0]
            };
            gradeId++;

            yield return newGrade;
        }
    }
}
