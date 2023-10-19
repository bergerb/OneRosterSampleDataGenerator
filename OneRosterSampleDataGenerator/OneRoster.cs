using Bogus;
using OneRosterSampleDataGenerator.Helpers;
using OneRosterSampleDataGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace OneRosterSampleDataGenerator;

/// <summary>
/// Complete Object of OneRoster Data generated from sample CSV files (random)
/// </summary>
/// <todo>
///   TODO: Check for file exists in all places
/// </todo>
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
    public List<Student> Students = new();
    public List<Staff> Staff = new();
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

    readonly Faker faker = new();
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

            void EnrollStudent(Student student, Enrollment enrollment)
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

        void DeactivateEnrollmentsForUser(IUser user)
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

        //write academic sessions
        string academicSessionsHeader = "sourcedId,status,dateLastModified,title,type,startDate,endDate,parentSourcedId,schoolYear";
        StringBuilder academicSessionsOutput = new();
        academicSessionsOutput.Append(academicSessionsHeader);
        foreach (AcademicSession a in AcademicSessions)
            academicSessionsOutput.Append($"{Environment.NewLine}\"{a.SourcedId}\",\"{a.Status}\",\"{FormatDateLastModified(a.DateLastModified)}\",\"{a.Title}\",\"{a.Type}\",\"{string.Format("{0:yyyy-MM-dd}", a.StartDate)}\",\"{string.Format("{0:yyyy-MM-dd}", a.EndDate)}\",\"\",\"{a.SchoolYear}\"");
        File.WriteAllText("OneRoster\\academicSessions.csv", academicSessionsOutput.ToString());
        StatusChangeBuilder.AddEvent(StatusChangeBuilder.EventAction.Created, StatusChangeBuilder.Type.File, "Academic Sessions");

        //write orgs
        string orgsHeader = "sourcedId,status,dateLastModified,name,type,identifier,parentSourcedId";
        StringBuilder orgsOutput = new();
        orgsOutput.Append(orgsHeader);
        foreach (Org o in Orgs)
            orgsOutput.Append($"{Environment.NewLine}\"{o.SourcedId}\",\"{o.Status}\",\"{FormatDateLastModified(o.DateLastModified)}\",\"{o.Name}\",\"{o.Type}\",\"{o.Identifier}\",\"{o.ParentSourcedId}\"");
        File.WriteAllText("OneRoster\\orgs.csv", orgsOutput.ToString());
        StatusChangeBuilder.AddEvent(StatusChangeBuilder.EventAction.Created, StatusChangeBuilder.Type.File, "Orgs");

        //write courses
        string coursesHeader = "sourcedId,status,dateLastModified,schoolYearSourcedId,title,courseCode,grades,orgSourcedId,subjects,subjectCodes";
        StringBuilder coursesOutput = new();
        coursesOutput.Append(coursesHeader);
        foreach (Course c in Courses)
            coursesOutput.Append($"{Environment.NewLine}\"{c.SourcedId}\",\"{c.Status}\",\"{FormatDateLastModified(c.DateLastModified)}\",\"{c.SchoolYearSourcedId}\",\"{c.Title}\",\"{c.CourseCode}\",\"{c.Grade.Name}\",\"{c.OrgSourcedId}\",\"\",\"\"");
        File.WriteAllText("OneRoster\\courses.csv", coursesOutput.ToString());
        StatusChangeBuilder.AddEvent(StatusChangeBuilder.EventAction.Created, StatusChangeBuilder.Type.File, "Courses");

        //write users
        string usersHeader = "sourcedId,status,dateLastModified,enabledUser,orgSourcedIds,role,username,userIds,givenName,familyName,middleName,identifier,email,sms,phone,agentSourcedIds,grades,password";
        StringBuilder usersOutput = new();
        usersOutput.Append(usersHeader);
        foreach (Student s in Students)
            usersOutput.Append($"{Environment.NewLine}\"{s.SourcedId}\",\"{s.Status}\",\"{FormatDateLastModified(s.DateLastModified)}\",\"true\",\"{s.Org.SourcedId}\",\"student\",\"{s.UserName}\",\"{s.Identifier}\",\"{s.GivenName}\",\"{s.FamilyName}\",\"\",\"{s.Identifier}\",\"{s.Email}\",\"\",\"\",\"\",\"{s.CurrentGrade}\",\"\"");
        foreach (Staff t in Staff)
            usersOutput.Append($"{Environment.NewLine}\"{t.SourcedId}\",\"{t.Status}\",\"{FormatDateLastModified(t.DateLastModified)}\",\"true\",\"{t.Org.SourcedId}\",\"{t.RoleType}\",\"{t.UserName}\",\"{t.Identifier}\",\"{t.GivenName}\",\"{t.FamilyName}\",\"\",\"{t.Identifier}\",\"{t.Email}\",\"\",\"\",\"\",\"\",\"\"");
        File.WriteAllText("OneRoster\\users.csv", usersOutput.ToString());
        StatusChangeBuilder.AddEvent(StatusChangeBuilder.EventAction.Created, StatusChangeBuilder.Type.File, "Users");

        //write classes
        string classesHeader = "sourcedId,status,dateLastModified,title,grades,courseSourcedId,classCode,classType,location,schoolSourcedId,termSourcedId,subjects,subjectCodes,periods";
        StringBuilder classesOutput = new();
        classesOutput.Append(classesHeader);
        foreach (Class c in Classes)
            classesOutput.Append($"{Environment.NewLine}\"{c.SourcedId}\",\"{c.Status}\",\"{FormatDateLastModified(c.DateLastModified)}\",\"{c.Title}\",\"{c.Grades}\",\"{c.CourseSourcedId}\",\"{c.ClassCode}\",\"{c.ClassType}\",\"\",\"{c.SchoolSourcedId}\",\"{c.TermSourcedid}\",\"\",\"\",\"\"");
        File.WriteAllText("OneRoster\\classes.csv", classesOutput.ToString());
        StatusChangeBuilder.AddEvent(StatusChangeBuilder.EventAction.Created, StatusChangeBuilder.Type.File, "Classes");

        //write enrollments
        string enrollmentsHeader = "sourcedId,status,dateLastModified,classSourcedId,schoolSourcedId,userSourcedId,role,primary,beginDate,endDate";
        StringBuilder enrollmentsOutput = new();
        enrollmentsOutput.Append(enrollmentsHeader);
        foreach (Enrollment e in Enrollments)
            enrollmentsOutput.Append($"{Environment.NewLine}\"{e.SourcedId}\",\"{e.Status}\",\"{FormatDateLastModified(e.DateLastModified)}\",\"{e.ClassSourcedId}\",\"{e.SchoolSourcedId}\",\"{e.UserSourcedId}\",\"{e.RoleType}\",\"\",\"\",\"\"");
        File.WriteAllText("OneRoster\\enrollments.csv", enrollmentsOutput.ToString());
        StatusChangeBuilder.AddEvent(StatusChangeBuilder.EventAction.Created, StatusChangeBuilder.Type.File, "Enrollments");

        //write demograhics
        string demographicsHeader = "sourcedId,status,dateLastModified,birthDate,sex,americanIndianOrAlaskaNative,asian,blackOrAfricanAmerican,nativeAmericanOrOtherPacificIslander,countryOfBirthCode,stateofBirthAbbreviation,cityOfBirth,publicSchoolResidenceStatus";
        StringBuilder demograhicsOutput = new();
        demograhicsOutput.Append(demographicsHeader);
        foreach (Demographic d in Demographics)
            demograhicsOutput.Append($"{Environment.NewLine}\"{d.SourcedId}\",\"{d.Status}\",\"{FormatDateLastModified(d.DateLastModified)}\",\"{string.Format("{0:yyyy-MM-dd}", d.BirthDate)}\",\"{d.Sex}\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"");
        File.WriteAllText("OneRoster\\demographics.csv", demograhicsOutput.ToString());
        StatusChangeBuilder.AddEvent(StatusChangeBuilder.EventAction.Created, StatusChangeBuilder.Type.File, "Demographics");

        //write manifest
        string manifestHeader = "\"propertyName\",\"value\"";
        StringBuilder manifestOutput = new();
        manifestOutput.Append(manifestHeader);
        foreach (Manifest manifest in Manifest)
            manifestOutput.Append($"{Environment.NewLine}\"{manifest.PropertyName}\",\"{manifest.Value}\"");
        File.WriteAllText("OneRoster\\manifest.csv", manifestOutput.ToString());
        StatusChangeBuilder.AddEvent(StatusChangeBuilder.EventAction.Created, StatusChangeBuilder.Type.File, "Manifest");

        #region Local Functions

        void SetupDirectory()
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

        string FormatDateLastModified(DateTime dateTime)
        {
            var formattedDateTime = dateTime.ToString("yyyy-MM-ddTHH:MM:ss.sssZ");
            return formattedDateTime;
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
    public Enrollment AddEnrollment(IUser user, Guid classSourcedId, Guid courseSourcedId, Guid schoolSourcedId, RoleType role)
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
    public Enrollment AddStudentEnrollment(Student student, Guid classSourcedId, Guid courseSourcedId, Guid schoolSourcedId)
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
