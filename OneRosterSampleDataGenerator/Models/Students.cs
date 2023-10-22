using Bogus;
using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class Students : Generator<User>
{
    private readonly Faker faker = new("en");

    public Students(DateTime createdAt, List<Org> orgs, List<Course> courses, int studentsPerGrade)
        : base(createdAt)
    {
        Orgs = orgs;
        Courses = courses;
        StudentsPerGrade = studentsPerGrade;
    }

    public List<Org> Orgs { get; set; }
    public List<Course> Courses { get; set; }
    public int StudentsPerGrade { get; set; }

    public override List<User> Generate()
    {
        Items = CreateStudents().ToList();

        return Items;
    }

    private IEnumerable<User> CreateStudents()
    {
        var rnd = new Random();

        foreach (Org org in Orgs.Where(e => e.OrgType == OrgType.school))
        {
            foreach (var grade in org.GradesOffer)
            {
                Random r = new();
                var CALC_NUM_STUDENTS_PER_GRADE = StudentsPerGrade + (r.Next(-30, 30));
                for (var i = 1; i < CALC_NUM_STUDENTS_PER_GRADE; i++)
                {
                    yield return addStudent(org, grade);
                }
            }
        }
    }

    public User AddStudent(Org org, Grade grade, DateTime? createdAt = null)
    {
        var student = addStudent(org, grade);

        if (createdAt.HasValue)
        {
            student.DateLastModified = (DateTime)createdAt;
        }

        AddItem(student);

        return student;
    }

    private User addStudent(Org org, Grade grade)
    {
        var student = new User
        {
            // Assign each student all courses of their current grade
            Courses = Courses.Where(e => e.Title.Contains(grade.Name)).ToList(),
            DateLastModified = CreatedAt,
            EnabledUser = true,
            FamilyName = faker.Name.LastName(),
            GivenName = faker.Name.FirstName(),
            Grade = grade,
            Identifier = RunningId.ToString(),
            Org = org,
            SourcedId = Guid.NewGuid(),
        };

        student.UserName = $"{student.GivenName[..1]}{student.FamilyName}{student.Identifier[^3..]}";

        RunningId++;

        return student;
    }

}
