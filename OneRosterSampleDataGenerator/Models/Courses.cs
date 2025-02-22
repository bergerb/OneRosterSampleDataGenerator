using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class Courses(DateTime createdAt, Org parentOrg, List<AcademicSession> academicSessions, List<Grade> grades) : Generator<Course>(createdAt)
{
    public Org ParentOrg { get; set; } = parentOrg;
    public List<AcademicSession> AcademicSessions { get; set; } = academicSessions;
    public List<Grade> Grades { get; set; } = grades;

    public override List<Course> Generate()
    {
        this.Items = this.CreateCourses().ToList();

        return this.Items;
    }

    public string? GetCourseTitle(Guid courseSourcedId)
    {
        return this.CreateCourses().ToList()
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
            var values = line?.Split(',') ?? [];
            var tmpGrade = values[1].ToString();
            var grade = tmpGrade.Substring(tmpGrade.Length - 2, 2);
            Course newCourse = new()
            {
                CourseCode = values[0],
                DateLastModified = this.CreatedAt,
                Grade = this.Grades.Where(e => e.Name.Contains(grade)).First(),
                OrgSourcedId = this.ParentOrg.SourcedId,
                SchoolYearSourcedId = this.AcademicSessions.Where(e => e.Title.Contains(values[2].ToString())).First().SourcedId,
                SourcedId = Guid.NewGuid(),
                Title = values[1],
            };

            yield return newCourse;
        }
    }
}
