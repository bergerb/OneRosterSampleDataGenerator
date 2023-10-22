using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class Courses : Generator<Course>
{
    public Courses(DateTime createdAt, Org parentOrg, List<AcademicSession> academicSessions, List<Grade> grades)
        : base(createdAt)
    {
        AcademicSessions = academicSessions;
        Grades = grades;
        ParentOrg = parentOrg;
    }

    public Org ParentOrg { get; set; }
    public List<AcademicSession> AcademicSessions { get; set; }
    public List<Grade> Grades { get; set; }

    public override List<Course> Generate()
    {
        Items = CreateCourses().ToList();

        return Items;
    }

    public string GetCourseTitle(Guid courseSourcedId)
    {
        return Items
            .Where(x => x.SourcedId == courseSourcedId)
            .FirstOrDefault()?
            .Title;
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
                CourseCode = values[0],
                DateLastModified = CreatedAt,
                Grade = Grades.Where(e => e.Name.Contains(grade)).First(),
                OrgSourcedId = ParentOrg.SourcedId,
                SchoolYearSourcedId = AcademicSessions.Where(e => e.Title.Contains(values[2].ToString())).FirstOrDefault().SourcedId,
                SourcedId = Guid.NewGuid(),
                Title = values[1],
            };

            yield return newCourse;
        }
    }
}
