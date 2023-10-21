﻿using OneRosterSampleDataGenerator.Helpers;
using OneRosterSampleDataGenerator.Models;
using OneRosterSampleDataGenerator.Models.Exports;
using OneRosterSampleDataGenerator.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace OneRosterSampleDataGenerator;

/// <summary>
/// Creates sample CSV files from data (random)
///  * Ability to create multiple files with incremental changes
/// </summary>
public class OneRoster
{
    public const int DEFAULT_NUM_SCHOOLS = 22;
    public const int DEFAULT_NUM_STUDENTS_PER_GRADE = 200;
    public const int DEFAULT_NUM_CLASS_SIZE = 20;
    public const int DEFAULT_NUM_MAX_TEACHER_CLASS_COUNT = 8;
    public const int DEFAULT_NUM_STUDENT_ID = 910000000;
    public const int DEFAULT_NUM_STAFF_ID = 1;
    private const string OutputDirectory = "OneRoster";
    private const string OutStatusChangeFileName = "OneRosterChanges.txt";
    private StatusChangeBuilder StatusChangeBuilder = new(OutStatusChangeFileName);

    public List<AcademicSession> AcademicSessions = new();
    public List<Grade> Grades = new();
    public List<Org> Orgs = new();
    public List<Course> Courses = new();
    public List<User> Students = new();
    public List<User> Staff = new();
    public List<Class> Classes = new();
    public List<Enrollment> Enrollments = new();
    public List<Demographic> Demographics = new();
    public List<Manifest> Manifest = new();

    public readonly Org ParentOrg = new()
    {
        SourcedId = Guid.NewGuid(),
        Name = "Solar School District",
        Identifier = "9999",
        OrgType = OrgType.district,
        ParentSourcedId = null
    };

    private readonly Args _args;
    private DateTime DateLastModified;

    public record Args()
    {
        public int ClassSize { get; init; } = DEFAULT_NUM_CLASS_SIZE;
        /// <summary>
        /// Creates a number of OneRoster from today to previous days.
        /// This will create given number of OneRoster files with updated date between each.
        /// </summary>
        public int? IncrementalDaysToCreate { get; init; } = null;
        public int SchoolCount { get; init; } = DEFAULT_NUM_SCHOOLS;
        public int StaffIdStart { get; init; } = DEFAULT_NUM_STAFF_ID;
        public int StudentIdStart { get; init; } = DEFAULT_NUM_STUDENT_ID;
        public int StudentsPerGrade { get; init; } = DEFAULT_NUM_STUDENTS_PER_GRADE;
        public int MaxTeacherClassCount { get; init; } = DEFAULT_NUM_MAX_TEACHER_CLASS_COUNT;

    }

    /// <summary>
    /// Generates in memory a randomly generated OneRoster construct
    /// </summary>
    /// <param name="args">Arguments Record</param>
    /// <exception cref="ArgumentException"></exception>
    public OneRoster(Args args = null)
    {
        _args = args ?? new Args();

        if (_args.SchoolCount <= 2)
        {
            throw new ArgumentException("`School Count` cannot be 2 or less.");
        }

        var daysToOffset = _args.IncrementalDaysToCreate ?? 0;
        DateLastModified = DateTime.Now.AddDays(daysToOffset * -1);

        // Generate Academic Sessions
        this.AcademicSessions = new AcademicSessions(DateLastModified)
            .Generate();

        // Build Grades
        this.Grades = new Grades()
            .Generate();

        // Build Orgs
        // -----> Orgs Rely on Grades
        this.Orgs = new Orgs(DateLastModified,
            _args.SchoolCount,
            ParentOrg,
            Grades)
            .Generate();

        // Build Course List
        this.Courses = new Courses(DateLastModified,
            ParentOrg,
            Grades,
            AcademicSessions)
            .Generate();

        // Build Students List
        var students = new Students(DateLastModified, _args.StudentsPerGrade, Courses, Orgs)
        {
            RunningId = _args.StudentIdStart
        };
        this.Students = students
            .Generate();

        var staff = new Staffs(DateLastModified, Orgs)
        {
            RunningId = _args.StaffIdStart
        };
        staff.GenerateAdministration();

        var enrollments = new Enrollments(DateLastModified);

        // Build Classes List
        this.Classes = new Classes(DateLastModified, _args.ClassSize, _args.MaxTeacherClassCount, Courses, Students, Orgs, staff, enrollments)
            .Generate();

        // Build Demographic List
        this.Demographics = new Demographics(DateLastModified, Students)
            .Generate();

        // Build Manifest List
        this.Manifest = new Manifests(ParentOrg)
            .Generate();

        Staff = staff.Generate();
        Enrollments = enrollments.Generate();

        if (_args.IncrementalDaysToCreate.HasValue)
        {
            CreateIncrementalFiles(students);
        }
    }

    private void CreateIncrementalFiles(Students students)
    {
        OutputOneRosterZipFile();

        for (int i = 1; i < _args.IncrementalDaysToCreate.Value; i++)
        {
            DateLastModified = DateLastModified.AddDays(1);

            for (int j = 0; j <= new Random().Next(0, 3); j++)
            {
                DeactivateRandomStudent();
            }
            for (int k = 0; k <= new Random().Next(0, 3); k++)
            {
                AddRandomStudent();
            }

            OutputOneRosterZipFile(i.ToString());

            StatusChangeBuilder.OutputChangeLog();
        }

        #region Local Functions

        void AddRandomStudent()
        {
            var org = Utility.GetRandomItem(Orgs.Where(x => x.OrgType == OrgType.school).ToList());
            var grade = Utility.GetRandomItem(org.GradesOffer);

            var existingStudent = Students
                .Where(x => x.Org.Id == org.Id)
                .Where(x => x.Grade.Id == grade.Id)
                .FirstOrDefault();

            var student = students.AddStudent(org, grade);
            StatusChangeBuilder.AddEvent(
                StatusChangeBuilder.EventAction.Created,
                StatusChangeBuilder.Type.Student,
                $"{student.FamilyName}, {student.GivenName} (Grade: {student.Grade.Name}) created at {org.Name}.");

            // Add enrollments
            if (existingStudent is not null)
            {
                var enrollments = Enrollments
                    .Where(x => x.UserSourcedId == existingStudent.SourcedId)
                    .ToList();
                foreach (var enrollment in enrollments)
                {
                    EnrollStudent(student, enrollment);
                }
            }

            #region Local Functions

            void EnrollStudent(User student, Enrollment enrollment)
            {
                AddStudentEnrollment(student, enrollment.ClassSourcedId, enrollment.CourseSourcedId, enrollment.SchoolSourcedId);

                var courseTitle = GetCourseTitle(enrollment.CourseSourcedId);
                StatusChangeBuilder.AddEvent(
                    StatusChangeBuilder.EventAction.Created,
                    StatusChangeBuilder.Type.Enrollment,
                    $"{student.FamilyName}, {student.GivenName} (Grade: {student.Grade.Name}) enrolled into {courseTitle}.");
            }

            #endregion
        }

        void DeactivateRandomStudent()
        {
            var randomStudent = new Random().Next(0, Students.Count - 1);
            var student = Students[randomStudent];
            Students[randomStudent] = StudentHelper.DeactivateStudent(student, DateLastModified);
            StatusChangeBuilder.AddEvent(
                StatusChangeBuilder.EventAction.Deactivated,
                StatusChangeBuilder.Type.Student,
                $"{student.FamilyName}, {student.GivenName} (Grade: {student.Grade.Name}) modified at {student.Org.Name}.");

            DeactivateEnrollmentsForUser(student);
        }

        void DeactivateEnrollmentsForUser(ILeaUser user)
        {
            var enrollments = Enrollments.Where(x => x.UserSourcedId == user.SourcedId)
            .ToList();
            foreach (var enrollment in enrollments)
            {
                EnrollmentHelper.InactivateEnrollment(enrollment, DateLastModified);
                var courseTitle = GetCourseTitle(enrollment.CourseSourcedId);
                StatusChangeBuilder.AddEvent(
                    StatusChangeBuilder.EventAction.Deactivated,
                    StatusChangeBuilder.Type.Enrollment,
                    $"{user.FamilyName}, {user.GivenName} enrollment has been deactivated for {courseTitle}.");
            }
        }

        #endregion

    }

    /// <summary>
    /// Generate All CSV Files Present
    /// </summary>
    public void OutputCSVFiles()
    {
        SetupDirectory();

        FileProcessor processor = new(StatusChangeBuilder);
        processor.ProcessFile<AcademicSession, AcademicSessionFile>(AcademicSessions, "OneRoster\\academicSessions.csv");

        processor.ProcessFile<Org, OrgFile>(Orgs, "OneRoster\\orgs.csv");

        processor.ProcessFile<Course, CourseFile>(Courses, "OneRoster\\courses.csv");

        var users = Students.Union(Staff);

        processor.ProcessFile<User, UserFile>(users, "OneRoster\\users.csv");

        processor.ProcessFile<Class, ClassFile>(Classes, "OneRoster\\classes.csv");

        processor.ProcessFile<Enrollment, EnrollmentFile>(Enrollments, "OneRoster\\enrollments.csv");

        processor.ProcessFile<Demographic, DemographicFile>(Demographics, "OneRoster\\demographics.csv");

        processor.ProcessFile<Manifest, ManifestFile>(Manifest, "OneRoster\\manifest.csv");

        StatusChangeBuilder.OutputChangeLog();
        #region Local Functions

        static void SetupDirectory()
        {
            if (Directory.Exists(OutputDirectory))
            {
                Directory.GetFiles(OutputDirectory)
                    .ToList()
                    .ForEach(x => File.Delete(x));
                return;
            }
            Directory.CreateDirectory(OutputDirectory);
        }

        #endregion
    }

    public void OutputOneRosterZipFile(string version = null)
    {
        OutputCSVFiles();

        string startPath = @".\OneRoster";
        string zipFile = $".\\OneRoster{version}.zip";

        if (File.Exists(zipFile))
        {
            File.Delete(zipFile);
        }

        ZipFile.CreateFromDirectory(startPath, zipFile);

        StatusChangeBuilder.AddEvent(StatusChangeBuilder.EventAction.Created, StatusChangeBuilder.Type.File, zipFile);

    }

    /// <summary>
    /// Add Enrollment for IUser for given class, course, and org
    /// </summary>
    /// <param name="user"></param>
    /// <param name="classSourcedId"></param>
    /// <param name="courseSourcedId"></param>
    /// <param name="schoolSourcedId"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    public Enrollment AddEnrollment(ILeaUser user, Guid classSourcedId, Guid courseSourcedId, Guid schoolSourcedId, RoleType role)
    {
        Enrollment enrollment = new()
        {
            DateLastModified = DateLastModified,
            ClassSourcedId = classSourcedId,
            CourseSourcedId = courseSourcedId,
            SchoolSourcedId = schoolSourcedId,
            SourcedId = Guid.NewGuid(),
            Status = StatusType.active,
            UserSourcedId = user.SourcedId,
            RoleType = role

        };
        Enrollments.Add(enrollment);
        return enrollment;
    }

    /// <summary>
    /// Add Student Enrollment
    /// </summary>
    /// <param name="student"></param>
    /// <param name="classSourcedId"></param>
    /// <param name="courseSourcedId"></param>
    /// <param name="schoolSourcedId"></param>
    /// <returns></returns>
    public Enrollment AddStudentEnrollment(User student, Guid classSourcedId, Guid courseSourcedId, Guid schoolSourcedId)
    {
        return AddEnrollment(student, classSourcedId, courseSourcedId, schoolSourcedId, RoleType.student);
    }

    #region "Courses"

    private string GetCourseTitle(Guid courseSourcedId)
    {
        return Courses
            .Where(x => x.SourcedId == courseSourcedId)
            .FirstOrDefault()?
            .Title;
    }

    #endregion
}
