using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class Classes(
    DateTime createdAt,
    int classSize,
    int maxTeacherClassCount,
    List<Org> orgs,
    List<Course> courses,
    List<User> students,
    Staffs staffs,
    Enrollments enrollments) : Generator<Class>(createdAt)
{
    public Enrollments Enrollments { get; set; } = enrollments;
    public int ClassSize { get; set; } = classSize;
    public int MaxTeacherClassCount { get; set; } = maxTeacherClassCount;
    public List<Course> Courses { get; set; } = courses;
    public List<Org> Orgs { get; set; } = orgs;
    public List<User> Students { get; set; } = students;
    public Staffs Staffs { get; set; } = staffs;

    public override List<Class> Generate()
    {
        this.Items = this.CreateClasses().ToList();

        return this.Items;
    }

    private IEnumerable<Class> CreateClasses()
    {
        foreach (Org org in this.Orgs.Where(e => e.OrgType == OrgType.school))
        {
            foreach (Grade grade in org.GradesOffer)
            {
                foreach (Course course in this.Courses.Where(e => e.Grade == grade))
                {
                    // Create new class after meeting class size
                    var students_ = from s in this.Students
                                    where s.Org.SourcedId == org.SourcedId &&
                                    s.Courses.Contains(course)
                                    select s;

                    // Determine how many class sections are needed
                    var classCount = (students_.Count() / this.ClassSize) + 1;

                    for (int i = 1; i <= classCount; i++)
                    {
                        string sectionNumber = i.ToString().PadLeft(3, '0');

                        Class @class = new()
                        {
                            ClassCode = org.Identifier + course.CourseCode + sectionNumber,
                            ClassType = (course.Title.Contains("HOMEROOM") ? IMSClassType.homeroom.ToString() : IMSClassType.scheduled.ToString()),
                            CourseSourcedId = course.SourcedId,
                            DateLastModified = this.CreatedAt,
                            Grades = grade.Name,
                            SchoolSourcedId = org.SourcedId,
                            SourcedId = Guid.NewGuid(),
                            Status = StatusType.active,
                            TermSourcedid = course.SchoolYearSourcedId,
                            Title = $"{course.Title} SEC {sectionNumber}",
                        };

                        yield return @class;

                        // Add Teacher
                        this.AddStaffToClass(@class, course, org);

                        // Add Students
                        this.AddStudentsToClass(i, students_, @class, course, org);
                    }
                }
            }
        }
    }

    public void AddStudentsToClass(int i, IEnumerable<User> students, Class @class, Course course, Org org)
    {
        foreach (User student in students.Skip((i - 1) * this.ClassSize).Take(this.ClassSize))
        {
            this.Enrollments.AddEnrollment(student, @class.SourcedId, course.SourcedId, org.SourcedId, RoleType.student);
        }
    }

    public void AddStaffToClass(Class @class, Course course, Org org)
    {
        User? teacher = null;
        // if class is homeroom add a new teacher
        //   every homeroom will have only one teacher
        if (course.Title.Contains("homeroom", StringComparison.CurrentCultureIgnoreCase))
        {
            teacher = this.Staffs.CreateStaff(org);
        }
        else
        {
            // Find an available teacher
            teacher = this.Staffs.Items
                .Where(e => e.Org == org && e.RoleType == RoleType.teacher
                    && e.Classes.Count < this.MaxTeacherClassCount)
                .First();
            // if no teachers are available
            //   make a new teacher
            teacher ??= this.Staffs.CreateStaff(org);
        }
        teacher.AddClass(@class);

        this.Enrollments.AddEnrollment(
            teacher,
            @class.SourcedId,
            course.SourcedId,
            org.SourcedId,
            RoleType.teacher);
    }
}

