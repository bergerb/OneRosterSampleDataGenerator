using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public record Courses(
    DateTime createdAt,
    Org parentOrg,
    List<Grade> grades,
    List<AcademicSession> academicSessions) : Generator<Course>
{
    public override List<Course> Generate()
    {
        Items = CreateCourses().ToList();

        return Items;
    }

    private IEnumerable<Course> CreateCourses()
    {
        using var reader = new StreamReader(Utility.StringToMemoryStream(Properties.Resources.courses));

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line.Split(',');
            var tmpGrade = values[1].ToString();
            var grade = tmpGrade.Substring(tmpGrade.Length - 2, 2);
            Course newCourse = new()
            {
                SourcedId = Guid.NewGuid(),
                Title = values[1],
                CourseCode = values[0],
                OrgSourcedId = parentOrg.SourcedId,
                SchoolYearSourcedId = academicSessions.Where(e => e.Title.Contains(values[2].ToString())).FirstOrDefault().SourcedId,
                Grade = grades.Where(e => e.Name.Contains(grade)).First()
            };

            yield return newCourse;
        }
    }
}
