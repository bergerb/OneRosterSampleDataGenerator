using Bogus;
using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public record Students(
    DateTime createdAt,
    int studentsPerGrade,
    List<Course> courses,
    List<Org> orgs) : Generator<Student>
{
    private readonly Faker faker = new("en");

    public override List<Student> Generate()
    {
        Items = CreateStudents().ToList();

        return Items;
    }

    private IEnumerable<Student> CreateStudents()
    {
        var rnd = new Random();

        foreach (Org org in orgs.Where(e => e.OrgType == OrgType.school))
        {
            foreach (var grade in org.GradesOffer)
            {
                Random r = new();
                var CALC_NUM_STUDENTS_PER_GRADE = studentsPerGrade + (r.Next(-30, 30));
                for (var i = 1; i < CALC_NUM_STUDENTS_PER_GRADE; i++)
                {
                    yield return AddStudent(org, grade);
                }
            }
        }
    }

    public Student AddStudent(Org org, Grade grade)
    {
        var student = new Student
        {
            SourcedId = Guid.NewGuid(),
            DateLastModified = createdAt,
            Identifier = RunningId.ToString(),
            EnabledUser = true,
            GivenName = faker.Name.FirstName(),
            FamilyName = faker.Name.LastName(),
            Grade = grade,
            Org = org,
            // Assign each student all courses of their current grade
            Courses = courses.Where(e => e.Title.Contains(grade.Name)).ToList()
        };

        RunningId++;

        return student;
    }

}
