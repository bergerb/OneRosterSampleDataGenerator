using OneRosterSampleDataGenerator.Helpers;
using OneRosterSampleDataGenerator.Models;
using OneRosterSampleDataGenerator.Models.Exports;
using OneRosterSampleDataGenerator.Services;
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
        this.Grades = new Grades(DateLastModified)
            .Generate();

        // Build Orgs
        // -----> Orgs Rely on Grades
        var orgs = new Orgs(DateLastModified,
            ParentOrg,
            _args.SchoolCount,
            Grades);
        this.Orgs = orgs
            .Generate();

        // Build Course List
        var courses = new Courses(
            DateLastModified,
            ParentOrg,
            AcademicSessions,
            Grades);
        this.Courses = courses.Generate();

        // Build Students List
        var students = new Students(DateLastModified, Orgs, Courses, _args.StudentsPerGrade)
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
        this.Classes = new Classes(
            DateLastModified,
            _args.ClassSize,
            _args.MaxTeacherClassCount,
            Orgs,
            Courses,
            Students,
            staff,
            enrollments)
            .Generate();

        // Build Demographic List
        this.Demographics = new Demographics(DateLastModified, Students)
            .Generate();

        // Build Manifest List
        this.Manifest = new Manifests(DateLastModified, ParentOrg)
            .Generate();

        Staff = staff.Generate();
        Enrollments = enrollments.Generate();

        if (_args.IncrementalDaysToCreate.HasValue)
        {
            CreateIncrementalFiles(students, enrollments, orgs, courses, StatusChangeBuilder);
        }
    }

    private void CreateIncrementalFiles(
        Students students,
        Enrollments enrollments,
        Orgs orgs,
        Courses courses,
        StatusChangeBuilder statusChangeBuilder)
    {
        OutputOneRosterZipFile();

        for (int i = 1; i < _args.IncrementalDaysToCreate.Value; i++)
        {
            DateLastModified = DateLastModified.AddDays(1);

            DeactivateStudentDataService deactivateStudentDataService = new(
                DateLastModified,
                students,
                enrollments,
                courses,
                statusChangeBuilder);
            var deactivated = deactivateStudentDataService
                .DeactivateStudents(3);

            students = deactivated.Students;
            enrollments = deactivated.Enrollments;

            AddStudentDataService incrementStudentDataService = new(
                DateLastModified,
                students,
                enrollments,
                orgs,
                courses,
                statusChangeBuilder);
            var added = incrementStudentDataService
                .AddStudents(3);
            students = added.Students;
            enrollments = added.Enrollments;

            Students = students.Items;
            Enrollments = enrollments.Items;

            OutputOneRosterZipFile(i.ToString());

            StatusChangeBuilder.OutputChangeLog();
        }
    }

    /// <summary>
    /// Generate All CSV Files Present
    /// </summary>
    public void OutputCSVFiles()
    {
        SetupDirectory();

        FileProcessor processor = new(StatusChangeBuilder);

        processor.ProcessFile<AcademicSession, AcademicSessionFile>(
            AcademicSessions,
            "OneRoster\\academicSessions.csv");
        processor.ProcessFile<Org, OrgFile>(
            Orgs,
            "OneRoster\\orgs.csv");
        processor.ProcessFile<Course, CourseFile>(
            Courses,
            "OneRoster\\courses.csv");

        var users = Students.Union(Staff);
        processor.ProcessFile<User, UserFile>(
            users,
            "OneRoster\\users.csv");

        processor.ProcessFile<Class, ClassFile>(
            Classes,
            "OneRoster\\classes.csv");

        processor.ProcessFile<Enrollment, EnrollmentFile>(
            Enrollments,
            "OneRoster\\enrollments.csv");

        processor.ProcessFile<Demographic, DemographicFile>(
            Demographics,
            "OneRoster\\demographics.csv");

        processor.ProcessFile<Manifest, ManifestFile>(
            Manifest,
            "OneRoster\\manifest.csv");

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
}
