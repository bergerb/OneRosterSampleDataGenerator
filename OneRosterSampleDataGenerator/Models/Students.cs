using Bogus;
using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class Students(DateTime createdAt, List<Org> orgs, List<Course> courses, int studentsPerGrade) : Generator<User>(createdAt)
{
    private readonly Faker faker = new("en");

    public List<Org> Orgs { get; set; } = orgs;
    public List<Course> Courses { get; set; } = courses;
    public int StudentsPerGrade { get; set; } = studentsPerGrade;

    public override List<User> Generate()
    {
        this.Items = this.CreateStudents().ToList();

        return this.Items;
    }

    private IEnumerable<User> CreateStudents()
    {
        foreach (Org org in this.Orgs.Where(e => e.OrgType == OrgType.school))
        {
            foreach (var grade in org.GradesOffer)
            {
                Random r = new();
                var CALC_NUM_STUDENTS_PER_GRADE = this.StudentsPerGrade + (r.Next(-30, 30));
                for (var i = 1; i < CALC_NUM_STUDENTS_PER_GRADE; i++)
                {
                    yield return this.CreateUser(org, grade);
                }
            }
        }
    }

    public User AddStudent(Org org, Grade grade, DateTime? createdAt = null)
    {
        var student = this.CreateUser(org, grade);

        if (createdAt.HasValue)
        {
            student.DateLastModified = (DateTime)createdAt;
        }

        this.AddItem(student);

        return student;
    }

    private User CreateUser(Org org, Grade grade)
    {
        var student = new User
        {
            // Assign each student all courses of their current grade
            Courses = this.Courses.Where(e => e.Title.Contains(grade.Name)).ToList(),
            DateLastModified = this.CreatedAt,
            EnabledUser = true,
            FamilyName = faker.Name.LastName(),
            GivenName = faker.Name.FirstName(),
            Grade = grade,
            Identifier = this.RunningId.ToString(),
            Org = org,
            RoleType = RoleType.student,
            SourcedId = Guid.NewGuid(),
        };

        student.UserName = $"{student.GivenName[..1]}{student.FamilyName}{student.Identifier[^3..]}";

        this.RunningId++;

        return student;
    }

}
