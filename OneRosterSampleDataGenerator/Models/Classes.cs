using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public record Classes(
    DateTime createdAt,
    int classSize,
    int maxTeacherClassCount,
    List<Course> courses,
    List<Student> students,
    List<Org> orgs,
    Staffs staff,
    Enrollments enrollments) : Generator<Class>
{
    public override List<Class> Generate()
    {
        Items = CreateClasses().ToList();

        return Items;
    }

    private IEnumerable<Class> CreateClasses()
    {
        foreach (Org org in orgs.Where(e => e.OrgType == OrgType.school))
        {
            foreach (Grade grade in org.GradesOffer)
            {
                foreach (Course course in courses.Where(e => e.Grade == grade))
                {
                    // Create new class after meeting class size
                    var students_ = from s in students
                                    where s.Org.SourcedId == org.SourcedId &&
                                    s.Courses.Contains(course)
                                    select s;

                    // Determine how many class sections are needed
                    var classCount = (students_.Count() / classSize) + 1;

                    for (int i = 1; i <= classCount; i++)
                    {
                        string sectionNumber = i.ToString().PadLeft(3, '0');

                        Class @class = new()
                        {
                            DateLastModified = createdAt,
                            SourcedId = Guid.NewGuid(),
                            Status = StatusType.active,
                            Grades = grade.Name,
                            CourseSourcedId = course.SourcedId,
                            Title = $"{course.Title} SEC {sectionNumber}",
                            ClassCode = org.Identifier + course.CourseCode + sectionNumber,
                            SchoolSourcedId = org.SourcedId,
                            TermSourcedid = course.SchoolYearSourcedId,
                            ClassType = (course.Title.Contains("HOMEROOM") ? IMSClassType.homeroom.ToString() : IMSClassType.scheduled.ToString())
                        };

                        yield return @class;

                        // Add Teacher
                        AddStaffToClass(@class, course, org);
                        // Add Students
                        AddStudentsToClass(i, students_, @class, course, org);

                    }
                }
            }
        }
    }
    public void AddStudentsToClass(int i, IEnumerable<Student> students, Class @class, Course course, Org org)
    {
        foreach (Student student in students.Skip((i - 1) * classSize).Take(classSize))
        {
            enrollments.AddEnrollment(student, @class.SourcedId, course.SourcedId, org.SourcedId, RoleType.student);
        }
    }

    public void AddStaffToClass(Class @class, Course course, Org org)
    {
        Staff teacher = null;
        // if class is homeroom add a new teacher
        //   every homeroom will have only one teacher
        if (course.Title.ToLower().Contains("homeroom"))
        {
            teacher = staff.CreateStaff(org);
        }
        else
        {
            // Find an available teacher
            teacher = staff.Items.Where(e => e.Org == org && e.RoleType == RoleType.teacher && e.Classes.Count() < maxTeacherClassCount).FirstOrDefault();
            // if no teachers are available
            //   make a new teacher
            teacher ??= staff.CreateStaff(org);
        }
        teacher.AddClass(@class);

        enrollments.AddEnrollment(teacher, @class.SourcedId, course.SourcedId, org.SourcedId, RoleType.teacher);
    }
}

