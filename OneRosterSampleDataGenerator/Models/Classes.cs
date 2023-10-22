using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class Classes : Generator<Class>
{
    public Classes(
        DateTime createdAt,
        int classSize,
        int maxTeacherClassCount,
        List<Org> orgs,
        List<Course> courses,
        List<User> students,
        Staffs staffs,
        Enrollments enrollments)
        : base(createdAt)
    {
        ClassSize = classSize;
        Courses = courses;
        Enrollments = enrollments;
        MaxTeacherClassCount = maxTeacherClassCount;
        Orgs = orgs;
        Staffs = staffs;
        Students = students;
    }

    public Enrollments Enrollments { get; set; }
    public int ClassSize { get; set; }
    public int MaxTeacherClassCount { get; set; }
    public List<Course> Courses { get; set; }
    public List<Org> Orgs { get; set; }
    public List<User> Students { get; set; }
    public Staffs Staffs { get; set; }

    public override List<Class> Generate()
    {
        Items = CreateClasses().ToList();

        return Items;
    }

    private IEnumerable<Class> CreateClasses()
    {
        foreach (Org org in Orgs.Where(e => e.OrgType == OrgType.school))
        {
            foreach (Grade grade in org.GradesOffer)
            {
                foreach (Course course in Courses.Where(e => e.Grade == grade))
                {
                    // Create new class after meeting class size
                    var students_ = from s in Students
                                    where s.Org.SourcedId == org.SourcedId &&
                                    s.Courses.Contains(course)
                                    select s;

                    // Determine how many class sections are needed
                    var classCount = (students_.Count() / ClassSize) + 1;

                    for (int i = 1; i <= classCount; i++)
                    {
                        string sectionNumber = i.ToString().PadLeft(3, '0');

                        Class @class = new()
                        {
                            ClassCode = org.Identifier + course.CourseCode + sectionNumber,
                            ClassType = (course.Title.Contains("HOMEROOM") ? IMSClassType.homeroom.ToString() : IMSClassType.scheduled.ToString()),
                            CourseSourcedId = course.SourcedId,
                            DateLastModified = CreatedAt,
                            Grades = grade.Name,
                            SchoolSourcedId = org.SourcedId,
                            SourcedId = Guid.NewGuid(),
                            Status = StatusType.active,
                            TermSourcedid = course.SchoolYearSourcedId,
                            Title = $"{course.Title} SEC {sectionNumber}",
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
    public void AddStudentsToClass(int i, IEnumerable<User> students, Class @class, Course course, Org org)
    {
        foreach (User student in students.Skip((i - 1) * ClassSize).Take(ClassSize))
        {
            Enrollments.AddEnrollment(student, @class.SourcedId, course.SourcedId, org.SourcedId, RoleType.student);
        }
    }

    public void AddStaffToClass(Class @class, Course course, Org org)
    {
        User teacher = null;
        // if class is homeroom add a new teacher
        //   every homeroom will have only one teacher
        if (course.Title.ToLower().Contains("homeroom"))
        {
            teacher = Staffs.CreateStaff(org);
        }
        else
        {
            // Find an available teacher
            teacher = Staffs.Items.Where(e => e.Org == org && e.RoleType == RoleType.teacher && e.Classes.Count < MaxTeacherClassCount).FirstOrDefault();
            // if no teachers are available
            //   make a new teacher
            teacher ??= Staffs.CreateStaff(org);
        }
        teacher.AddClass(@class);

        Enrollments.AddEnrollment(teacher, @class.SourcedId, course.SourcedId, org.SourcedId, RoleType.teacher);
    }
}

