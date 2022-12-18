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

    private int schoolCount = DEFAULT_NUM_SCHOOLS;
    private int studentsPerGrade = DEFAULT_NUM_STUDENTS_PER_GRADE;
    private int classSize = DEFAULT_NUM_CLASS_SIZE;
    private int maxTeacherClassSize = DEFAULT_NUM_MAX_TEACHER_CLASS_COUNT;
    private int studentIdStart = DEFAULT_NUM_STUDENT_ID;
    private int staffIdStart = DEFAULT_NUM_STAFF_ID;

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

    Org parentOrg = new Org
    {
        SourcedId = Guid.NewGuid(),
        Name = "Solar School District",
        Identifier = "9999",
        OrgType = OrgType.district,
        ParentSourcedId = null
    };

    Faker faker = new Faker();

    /// <summary>
    /// Generates in memory a randomly generated OneRoster construct
    /// </summary>
    /// <param name="schoolCount"></param>
    /// <param name="studentsPerGrade"></param>
    /// <param name="classSize"></param>
    /// <param name="maxTeacherClassCount"></param>
    /// <param name="studentIdStart"></param>
    /// <param name="staffIdStart"></param>
    public OneRoster(
            int? schoolCount = null,
            int? studentsPerGrade = null,
            int? classSize = null,
            int? maxTeacherClassCount = null,
            int? studentIdStart = null,
            int? staffIdStart = null)
    {
        if (schoolCount <= 2)
        {
            throw new ArgumentException("`School Count` cannot be 2 or less.");
        }
        SetParameters(
            schoolCount ?? DEFAULT_NUM_SCHOOLS,
            studentsPerGrade ?? DEFAULT_NUM_STUDENTS_PER_GRADE,
            classSize ?? DEFAULT_NUM_CLASS_SIZE,
            maxTeacherClassCount ?? DEFAULT_NUM_MAX_TEACHER_CLASS_COUNT,
            studentIdStart ?? DEFAULT_NUM_STUDENT_ID,
            staffIdStart ?? DEFAULT_NUM_STAFF_ID
            );
        // Generate Academic Sessions
        GenerateAcademicSessions();
        // Build Grades
        GenerateGrades();
        // Build Orgs
        // -----> Orgs Rely on Grades
        GenerateOrgs();
        // Build Course List
        GenerateCourses();
        // Build Students List
        GenerateStudents();
        // Build Classes List
        GenerateClasses();
        // Build Demographic List
        GenerateDemographics();
        // Build Manifest List
        GenerateManifest();
    }

    /// <summary>
    /// Set the default seeding parameters for the generated roster
    /// </summary>
    /// <param name="schoolCount"></param>
    /// <param name="studentsPerGrade"></param>
    /// <param name="classSize"></param>
    /// <param name="maxTeacherClassSize"></param>
    /// <param name="studentIdStart"></param>
    /// <param name="staffIdStart"></param>
    private void SetParameters(int schoolCount, int studentsPerGrade, int classSize, int maxTeacherClassSize, int studentIdStart, int staffIdStart)
    {
        this.schoolCount = schoolCount;
        this.studentsPerGrade = studentsPerGrade;
        this.classSize = classSize;
        this.maxTeacherClassSize = maxTeacherClassSize;
        this.studentIdStart = studentIdStart;
        this.staffIdStart = staffIdStart;
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

        //write orgs
        string orgsHeader = "sourcedId,status,dateLastModified,name,type,identifier,parentSourcedId";
        StringBuilder orgsOutput = new();
        orgsOutput.Append(orgsHeader);
        foreach (Org o in Orgs)
            orgsOutput.Append($"{Environment.NewLine}\"{o.SourcedId}\",\"{o.Status}\",\"{FormatDateLastModified(o.DateLastModified)}\",\"{o.Name}\",\"{o.Type}\",\"{o.Identifier}\",\"{o.ParentSourcedId}\"");
        File.WriteAllText("OneRoster\\orgs.csv", orgsOutput.ToString());

        //write courses
        string coursesHeader = "sourcedId,status,dateLastModified,schoolYearSourcedId,title,courseCode,grades,orgSourcedId,subjects,subjectCodes";
        StringBuilder coursesOutput = new();
        coursesOutput.Append(coursesHeader);
        foreach (Course c in Courses)
            coursesOutput.Append($"{Environment.NewLine}\"{c.SourcedId}\",\"{c.Status}\",\"{FormatDateLastModified(c.DateLastModified)}\",\"{c.SchoolYearSourcedId}\",\"{c.Title}\",\"{c.CourseCode}\",\"{c.Grade.Name}\",\"{c.OrgSourcedId}\",\"\",\"\"");
        File.WriteAllText("OneRoster\\courses.csv", coursesOutput.ToString());

        //write users
        string usersHeader = "sourcedId,status,dateLastModified,enabledUser,orgSourcedIds,role,username,userIds,givenName,familyName,middleName,identifier,email,sms,phone,agentSourcedIds,grades,password";
        StringBuilder usersOutput = new();
        usersOutput.Append(usersHeader);
        foreach (Student s in Students)
            usersOutput.Append($"{Environment.NewLine}\"{s.SourcedId}\",\"{s.Status}\",\"{FormatDateLastModified(s.DateLastModified)}\",\"true\",\"{s.Org.SourcedId}\",\"student\",\"{s.UserName}\",\"{s.Identifier}\",\"{s.GivenName}\",\"{s.FamilyName}\",\"\",\"{s.Identifier}\",\"{s.Email}\",\"\",\"\",\"\",\"{s.CurrentGrade}\",\"\"");
        foreach (Staff t in Staff)
            usersOutput.Append($"{Environment.NewLine}\"{t.SourcedId}\",\"{t.Status}\",\"{FormatDateLastModified(t.DateLastModified)}\",\"true\",\"{t.Org.SourcedId}\",\"{t.RoleType}\",\"{t.UserName}\",\"{t.Identifier}\",\"{t.GivenName}\",\"{t.FamilyName}\",\"\",\"{t.Identifier}\",\"{t.Email}\",\"\",\"\",\"\",\"\",\"\"");
        File.WriteAllText("OneRoster\\users.csv", usersOutput.ToString());

        //write classes
        string classesHeader = "sourcedId,status,dateLastModified,title,grades,courseSourcedId,classCode,classType,location,schoolSourcedId,termSourcedId,subjects,subjectCodes,periods";
        StringBuilder classesOutput = new();
        classesOutput.Append(classesHeader);
        foreach (Class c in Classes)
            classesOutput.Append($"{Environment.NewLine}\"{c.SourcedId}\",\"{c.Status}\",\"{FormatDateLastModified(c.DateLastModified)}\",\"{c.Title}\",\"{c.Grades}\",\"{c.CourseSourcedId}\",\"{c.ClassCode}\",\"{c.ClassType}\",\"\",\"{c.SchoolSourcedId}\",\"{c.TermSourcedid}\",\"\",\"\",\"\"");
        File.WriteAllText("OneRoster\\classes.csv", classesOutput.ToString());

        //write enrollments
        string enrollmentsHeader = "sourcedId,status,dateLastModified,classSourcedId,schoolSourcedId,userSourcedId,role,primary,beginDate,endDate";
        StringBuilder enrollmentsOutput = new();
        enrollmentsOutput.Append(enrollmentsHeader);
        foreach (Enrollment e in Enrollments)
            enrollmentsOutput.Append($"{Environment.NewLine}\"{e.SourcedId}\",\"{e.Status}\",\"{FormatDateLastModified(e.DateLastModified)}\",\"{e.ClassSourcedId}\",\"{e.SchoolSourcedId}\",\"{e.UserSourcedId}\",\"{e.RoleType}\",\"\",\"\",\"\"");
        File.WriteAllText("OneRoster\\enrollments.csv", enrollmentsOutput.ToString());

        //write demograhics
        string demographicsHeader = "sourcedId,status,dateLastModified,birthDate,sex,americanIndianOrAlaskaNative,asian,blackOrAfricanAmerican,nativeAmericanOrOtherPacificIslander,countryOfBirthCode,stateofBirthAbbreviation,cityOfBirth,publicSchoolResidenceStatus";
        StringBuilder demograhicsOutput = new();
        demograhicsOutput.Append(demographicsHeader);
        foreach (Demographic d in Demographics)
            demograhicsOutput.Append($"{Environment.NewLine}\"{d.SourcedId}\",\"{d.Status}\",\"{FormatDateLastModified(d.DateLastModified)}\",\"{string.Format("{0:yyyy-MM-dd}", d.BirthDate)}\",\"{d.Sex}\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"");
        File.WriteAllText("OneRoster\\demographics.csv", demograhicsOutput.ToString());

        //write manifest
        string manifestHeader = "\"propertyName\",\"value\"";
        StringBuilder manifestOutput = new();
        manifestOutput.Append(manifestHeader);
        foreach (Manifest manifest in Manifest)
            manifestOutput.Append($"{Environment.NewLine}\"{manifest.PropertyName}\",\"{manifest.Value}\"");
        File.WriteAllText("OneRoster\\manifest.csv", manifestOutput.ToString());

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

    public void OutputOneRosterZipFile()
    {
        OutputCSVFiles();

        string startPath = @".\OneRoster";
        string zipPath = @".\OneRoster.zip";

        if (File.Exists(zipPath))
        {
            File.Delete(zipPath);
        }

        ZipFile.CreateFromDirectory(startPath, zipPath);
    }


    #region "Manifest"
    /// <summary>
    /// Generate Manifest File
    /// </summary>
    public void GenerateManifest()
    {
        Manifest.Add(new Manifest() { PropertyName = "propertyName", Value = "value" });
        Manifest.Add(new Manifest() { PropertyName = "manifest.version", Value = "1.0" });
        Manifest.Add(new Manifest() { PropertyName = "oneroster.version", Value = "1.1" });
        Manifest.Add(new Manifest() { PropertyName = "source.systemName", Value = parentOrg.Name + " OneRoster" });
        Manifest.Add(new Manifest() { PropertyName = "source.systemCode", Value = parentOrg.Identifier });
        Manifest.Add(new Manifest() { PropertyName = "file.academicSessions", Value = "bulk" });
        Manifest.Add(new Manifest() { PropertyName = "file.orgs", Value = "bulk" });
        Manifest.Add(new Manifest() { PropertyName = "file.courses", Value = "bulk" });
        Manifest.Add(new Manifest() { PropertyName = "file.classes", Value = "bulk" });
        Manifest.Add(new Manifest() { PropertyName = "file.users", Value = "bulk" });
        Manifest.Add(new Manifest() { PropertyName = "file.enrollments", Value = "bulk" });
        Manifest.Add(new Manifest() { PropertyName = "file.demographics", Value = "bulk" });
        Manifest.Add(new Manifest() { PropertyName = "file.resources", Value = "absent" });
        Manifest.Add(new Manifest() { PropertyName = "file.classResources", Value = "absent" });
        Manifest.Add(new Manifest() { PropertyName = "file.courseResources", Value = "absent" });
        Manifest.Add(new Manifest() { PropertyName = "file.categories", Value = "absent" });
        Manifest.Add(new Manifest() { PropertyName = "file.lineItems", Value = "absent" });
        Manifest.Add(new Manifest() { PropertyName = "file.results", Value = "absent" });

    }
    #endregion

    #region "Demographics"
    public void GenerateDemographics()
    {
        foreach (Student student in this.Students)
        {
            var rnd = new Random();

            this.Demographics.Add(new Demographic()
            {
                SourcedId = student.SourcedId,
                Status = StatusType.active,
                BirthDate = DateTime.Parse($"7/1/{int.Parse(Utility.GetCurrentSchoolYear()) - (4 + student.Grade.Id)}")
                                .AddDays(rnd.Next(0, 365)),
                Sex = (rnd.Next(0, 1) == 0 ? "female" : "male"),
                CountryOfBirthCode = "",
                StateOfBirthAbbreviation = "",
                CityOfBirth = "",
                PublicSchoolResidenceStatus = ""
            });
        }
    }
    #endregion

    #region "Enrollments"
    /// <summary>
    /// Adds an enumeration of students to given class, course, and org
    /// </summary>
    /// <param name="i"></param>
    /// <param name="students"></param>
    /// <param name="class"></param>
    /// <param name="course"></param>
    /// <param name="org"></param>
    public void AddStudentsToClass(int i, IEnumerable<Student> students, Class @class, Course course, Org org)
    {
        foreach (Student student in students.Skip((i - 1) * classSize).Take(classSize))
        {
            addStudentEnrollment(student, @class.SourcedId, course.SourcedId, org.SourcedId);
        }
    }

    /// <summary>
    /// Add teacher to given class, course, and org
    /// </summary>
    /// <param name="class"></param>
    /// <param name="course"></param>
    /// <param name="org"></param>
    public void AddStaffToClass(Class @class, Course course, Org org)
    {
        Staff teacher = null;
        // if class is homeroom add a new teacher
        //   every homeroom will have only one teacher
        if (course.Title.ToLower().Contains("homeroom"))
        {
            teacher = CreateStaff(org);
        }
        else
        {
            // Find an available teacher
            teacher = Staff.Where(e => e.Org == org && e.RoleType == RoleType.teacher && e.Classes.Count() < maxTeacherClassSize).FirstOrDefault();
            // if no teachers are available
            //   make a new teacher
            if (teacher == null)
                teacher = CreateStaff(org);
        }
        teacher.AddClass(@class);
        addTeacherEnrollment(teacher, @class.SourcedId, course.SourcedId, org.SourcedId);

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
    public Enrollment addEnrollment(IUser user, Guid classSourcedId, Guid courseSourcedId, Guid schoolSourcedId, RoleType role)
    {
        Enrollment enrollment = new Enrollment
        {
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
    /// Add Teacher Enrollment
    /// </summary>
    /// <param name="teacher"></param>
    /// <param name="classSourcedId"></param>
    /// <param name="courseSourcedId"></param>
    /// <param name="schoolSourcedId"></param>
    /// <returns></returns>
    public Enrollment addTeacherEnrollment(Staff teacher, Guid classSourcedId, Guid courseSourcedId, Guid schoolSourcedId)
    {
        return addEnrollment(teacher, classSourcedId, courseSourcedId, schoolSourcedId, RoleType.teacher);
    }

    /// <summary>
    /// Add Student Enrollment
    /// </summary>
    /// <param name="student"></param>
    /// <param name="classSourcedId"></param>
    /// <param name="courseSourcedId"></param>
    /// <param name="schoolSourcedId"></param>
    /// <returns></returns>
    public Enrollment addStudentEnrollment(Student student, Guid classSourcedId, Guid courseSourcedId, Guid schoolSourcedId)
    {
        return addEnrollment(student, classSourcedId, courseSourcedId, schoolSourcedId, RoleType.student);
    }
    #endregion

    #region "Classes"
    /// <summary>
    /// Start Class Generation Process
    /// </summary>
    public void GenerateClasses()
    {
        foreach (Org org in Orgs.Where(e => e.OrgType == OrgType.school))
        {
            for (int i = 0; i < (org.IsHigh ? 3 : org.IsMiddle ? 2 : org.IsElementary ? 1 : 1); i++)
            {
                CreateStaff(org, RoleType.administrator);
            }

            foreach (Grade grade in org.GradesOffer)
            {
                foreach (Course course in Courses.Where(e => e.Grade == grade))
                {
                    // Create new class after meeting class size
                    var students = from s in this.Students
                                   where s.Org.SourcedId == org.SourcedId &&
                                   s.Courses.Contains(course)
                                   select s;

                    // Determine how many class sections are needed
                    var classCount = (students.Count() / classSize) + 1;

                    for (int i = 1; i <= classCount; i++)
                    {
                        string sectionNumber = i.ToString().PadLeft(3, '0');

                        Class @class = new()
                        {
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
                        Classes.Add(@class);

                        // Add Teacher
                        AddStaffToClass(@class, course, org);
                        // Add students
                        AddStudentsToClass(i, students, @class, course, org);

                    }
                }
            }
        }
    }
    #endregion

    #region "Academic Sessions"
    /// <summary>
    /// Star Academic Sessions Generation
    /// </summary>
    private void GenerateAcademicSessions()
    {
        // Get Current School Year
        var schoolYear = Utility.GetCurrentSchoolYear();
        var nextSchoolYear = Utility.GetNextSchoolYear();
        // Create SchoolYear Term
        AcademicSession academicSession = new()
        {
            SourcedId = Guid.NewGuid(),
            Status = StatusType.active,
            Title = $"FY {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"8/16/{schoolYear}"),
            EndDate = DateTime.Parse($"8/15/{nextSchoolYear}"),
            SessionType = SessionType.schoolYear,
            SchoolYear = schoolYear
        };
        this.AcademicSessions.Add(academicSession);

        // Marking Periods
        AcademicSession academicSessionMP1 = new()
        {
            SourcedId = Guid.NewGuid(),
            Status = StatusType.active,
            Title = $"MP1 {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"8/30/{schoolYear}"),
            EndDate = DateTime.Parse($"11/09/{schoolYear}"),
            SessionType = SessionType.gradingPeriod,
            SchoolYear = schoolYear
        };
        this.AcademicSessions.Add(academicSessionMP1);

        AcademicSession academicSessionMP2 = new()
        {
            SourcedId = Guid.NewGuid(),
            Status = StatusType.active,
            Title = $"MP2 {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"11/10/{schoolYear}"),
            EndDate = DateTime.Parse($"01/29/{nextSchoolYear}"),
            SessionType = SessionType.gradingPeriod,
            SchoolYear = schoolYear
        };
        this.AcademicSessions.Add(academicSessionMP2);

        AcademicSession academicSessionMP3 = new()
        {
            SourcedId = Guid.NewGuid(),
            Status = StatusType.active,
            Title = $"MP3 {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"01/30/{nextSchoolYear}"),
            EndDate = DateTime.Parse($"04/13/{nextSchoolYear}"),
            SessionType = SessionType.gradingPeriod,
            SchoolYear = schoolYear
        };
        this.AcademicSessions.Add(academicSessionMP3);

        AcademicSession academicSessionMP4 = new()
        {
            SourcedId = Guid.NewGuid(),
            Status = StatusType.active,
            Title = $"MP4 {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"4/14/{nextSchoolYear}"),
            EndDate = DateTime.Parse($"6/30/{nextSchoolYear}"),
            SessionType = SessionType.gradingPeriod,
            SchoolYear = schoolYear
        };
        this.AcademicSessions.Add(academicSessionMP4);

        // Semesters

        AcademicSession academicSessionS1 = new()
        {
            SourcedId = Guid.NewGuid(),
            Status = StatusType.active,
            Title = $"S1 {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"8/30/{schoolYear}"),
            EndDate = DateTime.Parse($"1/29/{nextSchoolYear}"),
            SessionType = SessionType.term,
            SchoolYear = schoolYear
        };
        this.AcademicSessions.Add(academicSessionS1);

        AcademicSession academicSessionS2 = new()
        {
            SourcedId = Guid.NewGuid(),
            Status = StatusType.active,
            Title = $"S2 {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"1/30/{nextSchoolYear}"),
            EndDate = DateTime.Parse($"6/30/{nextSchoolYear}"),
            SessionType = SessionType.term,
            SchoolYear = schoolYear
        };
        this.AcademicSessions.Add(academicSessionS2);

        AcademicSession academicSessionSummer = new()
        {
            SourcedId = Guid.NewGuid(),
            Status = StatusType.active,
            Title = $"Summer {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"6/30/{nextSchoolYear}"),
            EndDate = DateTime.Parse($"8/15/{nextSchoolYear}"),
            SessionType = SessionType.semester,
            SchoolYear = schoolYear
        };
        this.AcademicSessions.Add(academicSessionSummer);
    }
    #endregion

    #region "Teacher"
    /// <summary>
    /// Creates a Teacher Record
    /// </summary>
    /// <returns></returns>
    public Staff CreateStaff(Org org = null, RoleType roleType = RoleType.teacher)
    {
        string[] teacherNames = Encoding.
              ASCII.
              GetString(Utility.StringToMemoryStream(Properties.Resources.teachers).ToArray()).
              Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        var maxTeachers = teacherNames.Count();
        var rnd = new Random();

        var staffid = "00000000" + staffIdStart.ToString();

        Staff newStaff = new()
        {
            SourcedId = Guid.NewGuid(),
            Identifier = staffid.Substring(staffid.Length - 8, 8),
            EnabledUser = true,
            GivenName = faker.Name.FirstName(),
            FamilyName = faker.Name.LastName(),
            RoleType = roleType,
            Org = org
        };
        newStaff.UserName = Utility.CreateTeacherUserName(Staff, newStaff.GivenName, newStaff.FamilyName);
        staffIdStart++;
        Staff.Add(newStaff);
        return newStaff;
    }

    #endregion

    #region "Students"
    /// <summary>
    /// Start Student Generation
    /// </summary>
    void GenerateStudents()
    {
        string[] firstNames = Encoding.
                  ASCII.
                  GetString(Utility.StringToMemoryStream(Properties.Resources.firstnames).ToArray()).
                  Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        string[] lastNames = Encoding.
                  ASCII.
                  GetString(Utility.StringToMemoryStream(Properties.Resources.lastnames).ToArray()).
                  Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        var rnd = new Random();
        var maxFirstNames = firstNames.Length;
        var maxLastNames = lastNames.Length;

        foreach (Org org in Orgs.Where(e => e.OrgType == OrgType.school))
        {
            foreach (var grade in org.GradesOffer)
            {
                Random r = new();
                var CALC_NUM_STUDENTS_PER_GRADE = studentsPerGrade + (r.Next(-30, 30));
                for (var i = 1; i < CALC_NUM_STUDENTS_PER_GRADE; i++)
                {
                    var FName = rnd.Next(0, maxFirstNames);
                    var LName = rnd.Next(0, maxLastNames);
                    var stu = new Student
                    {
                        SourcedId = Guid.NewGuid(),
                        Identifier = studentIdStart.ToString(),
                        EnabledUser = true,
                        GivenName = firstNames[FName].ToString(),
                        FamilyName = lastNames[LName].ToString(),
                        Grade = grade,
                        Org = org,
                        // Assign each student all courses of their current grade
                        Courses = Courses.Where(e => e.Title.Contains(grade.Name)).ToList()
                    };
                    Students.Add(stu);
                    studentIdStart++;
                }
            }
        }
    }
    #endregion

    #region "Grades"
    /// <summary>
    /// Start Grade Generation
    /// </summary>
    void GenerateGrades()
    {
        using var reader = new StreamReader(Utility.StringToMemoryStream(Properties.Resources.grades));

        int gradeId = 1;
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line.Split(',');
            Grade newGrade = new Grade
            {
                Id = gradeId,
                Name = values[0]
            };
            gradeId++;
            Grades.Add(newGrade);
        }
    }
    #endregion

    #region "Orgs"
    /// <summary>
    /// Generate Orgs
    /// </summary>
    void GenerateOrgs()
    {
        Orgs.Add(parentOrg);

        string[] schools = Encoding.
                  ASCII.
                  GetString(Utility.StringToMemoryStream(Properties.Resources.orgs).ToArray()).
                  Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        var maxSchools = schools.Count() - 1;
        var rnd = new Random();

        var randomSeq = Enumerable.Range(1, maxSchools).OrderBy(r => rnd.NextDouble()).Take(schoolCount).ToList();
        string[] schoolTypes = { "Elementary School", "Elementary School", "Middle School", "Middle School", "High School" };

        for (int count = 0; count < randomSeq.Count(); count++)
        {
            string line = schools[randomSeq[count]];
            var paddedOrgNum = ("0000" + randomSeq[count].ToString());
            var identifier = paddedOrgNum.Substring(paddedOrgNum.Length - 4, 4);
            var schoolName = schoolCount != 3 ?
                $"{line} {schoolTypes[rnd.Next(schoolTypes.Length)]}" :
                $"{line} {GradeHelper.SchoolLevels[count]}";

            Orgs.Add(
                OrgHelper.CreateSchool(
                    identifier,
                    schoolName,
                    parentOrg.SourcedId,
                    Grades
                    )
                );
        }

    }

    #endregion

    #region "Courses"
    /// <summary>
    /// Start Course Generation
    /// </summary>
    void GenerateCourses()
    {
        using var reader = new StreamReader(Utility.StringToMemoryStream(Properties.Resources.courses));

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line.Split(',');
            var tmpGrade = values[1].ToString();
            var grade = tmpGrade.Substring(tmpGrade.Length - 2, 2);
            Course newCourse = new()
            {
                SourcedId = Guid.NewGuid(),
                Title = values[1],
                CourseCode = values[0],
                OrgSourcedId = parentOrg.SourcedId,
                SchoolYearSourcedId = this.AcademicSessions.Where(e => e.Title.Contains(values[2].ToString())).FirstOrDefault().SourcedId,
                Grade = Grades.Where(e => e.Name.Contains(grade)).First()
            };
            Courses.Add(newCourse);
        }

    }
    #endregion
}
