using OneRosterSampleDataGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Compression;

namespace OneRosterSampleDataGenerator
{
    /// <summary>
    /// Complete Object of OneRoster Data generated from sample CSV files (random)
    /// </summary>
    /// <todo>
    ///   TODO: Check for file exists in all places
    /// </todo>
    public class OneRoster
    {
        int NUM_SCHOOLS = 22;
        int NUM_STUDENTS_PER_GRADE = 200;

        int NUM_CLASS_SIZE = 20;
        int NUM_MAX_TEACHER_CLASS_COUNT = 8;

        int NUM_STUDENT_ID = 910000000;

        int NUM_STAFF_ID = 1;

        string GRADES = "ALL";
        public List<AcademicSession> academicSessions = new List<AcademicSession>();
        public List<Grade> grades = new List<Grade>();
        public List<Org> orgs = new List<Org>();
        public List<Course> courses = new List<Course>();
        public List<Student> students = new List<Student>();
        public List<Teacher> teachers = new List<Teacher>();
        public List<Class> classes = new List<Class>();
        public List<Enrollment> enrollments = new List<Enrollment>();
        public List<Demographic> demographics = new List<Demographic>();
        public List<Manifest> manifest = new List<Manifest>();
        Org parentOrg = new Org
        {
            sourcedId = Guid.NewGuid(),
            name = "Solar School District",
            identifier = "9999",
            orgType = OrgType.district,
            parentSourcedId = null
        };

        const string GRADES_FILE = @"../../../../Templates/planets/grades.csv";
        const string ORGS_FILE = @"../../../../Templates/planets/orgs.csv";
        const string COURSES_FILE = @"../../../../Templates/planets/courses.csv";
        const string STUDENT_FIRSTNAME_FILE = @"../../../../Templates/planets/firstnames.csv";
        const string STUDENT_LASTNAME_FILE = @"../../../../Templates/planets/lastnames.csv";
        const string TEACHERS_FILE = @"../../../../Templates/planets/teachers.csv";

        string[] elemGrades = "KG,01,02,03,04,05".Split(',');
        string[] middleGrades = "06,07,08".Split(',');
        string[] highGrades = "09,10,11,12".Split(',');

        /// <summary>
        /// Instantiate OneRoster 
        /// </summary>
        public OneRoster()
        {
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
        /// Generate All CSV Files Present
        /// </summary>
        public void outputCSVFiles()
        {
            //write academic sessions
            string academicSessionsHeader = "sourcedId,status,dateLastModified,title,type,startDate,endDate,parentSourcedId,schoolYear";
            StringBuilder academicSessionsOutput = new StringBuilder();
            academicSessionsOutput.Append(academicSessionsHeader);
            foreach (AcademicSession a in academicSessions)
                academicSessionsOutput.Append($"{Environment.NewLine}\"{a.sourcedId}\",\"\",\"\",\"{a.title}\",\"{a.type}\",\"{string.Format("{0:yyyy-MM-dd}", a.startDate)}\",\"{string.Format("{0:yyyy-MM-dd}", a.endDate)}\",\"\",\"{a.schoolYear}\"");
            File.WriteAllText("academicSessions.csv", academicSessionsOutput.ToString());

            //write orgs
            string orgsHeader = "sourcedId,status,dateLastModified,name,type,identifier,parentSourcedId";
            StringBuilder orgsOutput = new StringBuilder();
            orgsOutput.Append(orgsHeader);
            foreach (Org o in orgs)
                orgsOutput.Append($"{Environment.NewLine}\"{o.sourcedId}\",\"\",\"\",\"{o.name}\",\"{o.type}\",\"{o.identifier}\",\"{o.parentSourcedId}\"");
            File.WriteAllText("orgs.csv", orgsOutput.ToString());

            //write courses
            string coursesHeader = "sourcedId,status,dateLastModified,metadata,title,classCode,classType,location,grades,subjects,course,school,term,subjectCodes,period,resources";
            StringBuilder coursesOutput = new StringBuilder();
            coursesOutput.Append(coursesHeader);
            foreach (Course c in courses)
                coursesOutput.Append($"{Environment.NewLine}\"{c.sourcedId}\",\"\",\"\",\"{c.title}\",\"{c.courseCode}\",\"\",\"{c.grade.name}\",\"\",\"{c.courseCode}\",\"\",\"{c.orgSourcedId}\",\"\",\"\",\"\"");
            File.WriteAllText("courses.csv", coursesOutput.ToString());

            //write users
            string usersHeader = "sourcedId,status,dateLastModified,enabledUser,orgSourcedIds,role,username,userIds,givenName,familyName,middleName,identifier,email,sms,phone,agentSourcedIds,grades,password";
            StringBuilder usersOutput = new StringBuilder();
            usersOutput.Append(usersHeader);
            foreach (Student s in students)
                usersOutput.Append($"{Environment.NewLine}\"{s.sourcedId}\",\"\",\"\",\"true\",\"{s.org.sourcedId}\",\"student\",\"{s.userName}\",\"{s.identifier}\",\"{s.givenName}\",\"{s.familyName}\",\"\",\"{s.identifier}\",\"{s.email}\",\"\",\"\",\"\",\"{s.currentGrade}\",\"\"");
            foreach (Teacher t in teachers)
                usersOutput.Append($"{Environment.NewLine}\"{t.sourcedId}\",\"\",\"\",\"true\",\"{t.org.sourcedId}\",\"teacher\",\"{t.userName}\",\"{t.identifier}\",\"{t.givenName}\",\"{t.familyName}\",\"\",\"{t.identifier}\",\"{t.email}\",\"\",\"\",\"\",\"\",\"\"");
            File.WriteAllText("users.csv", usersOutput.ToString());

            //write classes
            string classesHeader = "sourcedId,dateLastModified,title,grades,courseSourcedId,classCode,classType,location,schoolSourcedId,termSourcedId,subjects,subjectCodes,periods";
            StringBuilder classesOutput = new StringBuilder();
            classesOutput.Append(classesHeader);
            foreach (Class c in classes)
                classesOutput.Append($"{Environment.NewLine}\"{c.sourcedId}\",\"\",\"\",\"{c.grades}\",\"{c.courseSourcedId}\",\"{c.classCode}\",\"{c.classType}\",\"\",\"{c.schoolSourcedId}\",\"{c.termSourcedid}\",\"\",\"\",\"\"");
            File.WriteAllText("classes.csv", classesOutput.ToString());

            //write demograhics
            string demographicsHeader = "sourcedId,status,dateLastModified,birthDate,sex,americanIndianOrAlaskaNative,asian,blackOrAfricanAmerican,nativeAmericanOrOtherPacificIslander,countryOfBirthCode,stateofBirthAbbreviation,cityOfBirth,publicSchoolResidenceStatus";
            StringBuilder demograhicsOutput = new StringBuilder();
            demograhicsOutput.Append(demographicsHeader);
            foreach (Demographic d in demographics)
                demograhicsOutput.Append($"{Environment.NewLine}\"{d.sourcedId}\",\"\",\"\",\"{string.Format("{0:yyyy-MM-dd}", d.birthDate)}\",\"{d.sex}\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"");
            File.WriteAllText("demographics.csv", demograhicsOutput.ToString());

            //write manifest
            string manifestHeader = "\"propertyName\",\"value\"";
            StringBuilder manifestOutput = new StringBuilder();
            manifestOutput.Append(manifestHeader);
            foreach (Manifest manifest in manifest)
                manifestOutput.Append($"{Environment.NewLine}\"{manifest.propertyName}\",\"{manifest.value}\"");
            File.WriteAllText("manifest.csv", demograhicsOutput.ToString());
        }

        #region "Manifest"
        /// <summary>
        /// Generate Manifest File
        /// </summary>
        public void GenerateManifest()
        {
            manifest.Add(new Manifest() { propertyName = "propertyName", value = "value" });
            manifest.Add(new Manifest() { propertyName = "manifest.version", value = "1.0" });
            manifest.Add(new Manifest() { propertyName = "oneroster.version", value = "1.1" });
            manifest.Add(new Manifest() { propertyName = "source.systemName", value = parentOrg.name + " OneRoster" });
            manifest.Add(new Manifest() { propertyName = "source.systemCode", value = parentOrg.identifier });
            manifest.Add(new Manifest() { propertyName = "file.academicSessions", value = "bulk" });
            manifest.Add(new Manifest() { propertyName = "file.orgs", value = "bulk" });
            manifest.Add(new Manifest() { propertyName = "file.courses", value = "bulk" });
            manifest.Add(new Manifest() { propertyName = "file.classes", value = "bulk" });
            manifest.Add(new Manifest() { propertyName = "file.users", value = "bulk" });
            manifest.Add(new Manifest() { propertyName = "file.enrollments", value = "bulk" });
            manifest.Add(new Manifest() { propertyName = "file.demographics", value = "bulk" });
            manifest.Add(new Manifest() { propertyName = "file.resources", value = "absent" });
            manifest.Add(new Manifest() { propertyName = "file.classResources", value = "absent" });
            manifest.Add(new Manifest() { propertyName = "file.courseResources", value = "absent" });
            manifest.Add(new Manifest() { propertyName = "file.categories", value = "absent" });
            manifest.Add(new Manifest() { propertyName = "file.lineItems", value = "absent" });
            manifest.Add(new Manifest() { propertyName = "file.results", value = "absent" });

        }
        #endregion

        #region "Demographics"
        public void GenerateDemographics()
        {
            foreach (Student student in this.students)
            {
                var rnd = new Random();

                this.demographics.Add(new Demographic()
                {
                    sourcedId = student.sourcedId,
                    Status = StatusType.active,
                    CreatedAt = DateTime.Now,
                    birthDate = DateTime.Parse((rnd.Next(1, 12)).ToString() + "/" + (rnd.Next(1, 28)).ToString() + "/" + DateTime.Now.AddYears((6 + student.grade.id) * -1).Year.ToString()),
                    sex = (rnd.Next(0, 2) == 0 ? "female" : "male"),
                    countryOfBirthCode = "",
                    stateOfBirthAbbreviation = "",
                    cityOfBirth = "",
                    publicSchoolResidenceStatus = ""
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
            foreach (Student student in students.Skip((i - 1) * NUM_CLASS_SIZE).Take(NUM_CLASS_SIZE))
            {
                addStudentEnrollment(student, @class.sourcedId, course.sourcedId, org.sourcedId);
            }
        }

        /// <summary>
        /// Add teacher to given class, course, and org
        /// </summary>
        /// <param name="class"></param>
        /// <param name="course"></param>
        /// <param name="org"></param>
        public void AddTeacherToClass(Class @class, Course course, Org org)
        {
            Teacher teacher = null;
            // if class is homeroom add a new teacher
            //   every homeroom will have only one teacher
            if (course.title.ToLower().Contains("homeroom"))
            {
                teacher = CreateTeacher(org);
            }
            else // Find available teacher
            {
                // Find an available teacher
                teacher = teachers.Where(e => e.org == org && e.classes.Count() < NUM_MAX_TEACHER_CLASS_COUNT).FirstOrDefault();
                // if no teachers are available
                //   make a new teacher
                if (teacher == null)
                    teacher = CreateTeacher(org);
            }
            teacher.AddClass(@class);
            addTeacherEnrollment(teacher, @class.sourcedId, course.sourcedId, org.sourcedId);

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
                sourcedId = Guid.NewGuid(),
                Status = StatusType.active,
                classSourcedId = classSourcedId,
                courseSourcedId = courseSourcedId,
                schoolSourcedId = schoolSourcedId,
                userSourcedId = user.sourcedId,
                Role = role

            };
            enrollments.Add(enrollment);
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
        public Enrollment addTeacherEnrollment(Teacher teacher, Guid classSourcedId, Guid courseSourcedId, Guid schoolSourcedId)
        {
            return addEnrollment(teacher, classSourcedId, courseSourcedId, schoolSourcedId, RoleType.teacher);
        }

        /// <summary>
        /// Add Student Enrollmen t
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
            foreach (Org org in orgs.Where(e => e.orgType == OrgType.school))
            {
                foreach (Grade grade in org.gradesOffer)
                {
                    foreach (Course course in courses.Where(e => e.grade == grade))
                    {
                        // Create new class after meeting class size
                        var students = (from s in this.students
                                        where s.org.sourcedId == org.sourcedId &&
                                        s.courses.Contains(course)
                                        select s);

                        // Determine how many class sections are needed
                        var classCount = (students.Count() / NUM_CLASS_SIZE) + 1;

                        for (int i = 1; i <= classCount; i++)
                        {
                            string sectionNumber = i.ToString().PadLeft(3, '0');

                            Class @class = new Class
                            {
                                sourcedId = Guid.NewGuid(),
                                Status = StatusType.active,
                                grades = grade.name,
                                courseSourcedId = course.sourcedId,
                                title = $"{course.title} SEC {sectionNumber}",
                                classCode = org.identifier + course.courseCode + sectionNumber,
                                schoolSourcedId = org.sourcedId,
                                termSourcedid = course.schoolYearSourcedId,
                                classType = (course.title.Contains("HOMEROOM") ? IMSClassType.homeroom.ToString() : IMSClassType.scheduled.ToString())
                            };
                            classes.Add(@class);

                            // Add Teacher
                            AddTeacherToClass(@class, course, org);
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
            AcademicSession academicSession = new AcademicSession
            {
                sourcedId = Guid.NewGuid(),
                Status = StatusType.active,
                title = $"FY {schoolYear}-{nextSchoolYear}",
                startDate = DateTime.Parse($"8/30/{schoolYear}"),
                endDate = DateTime.Parse($"6/30/{nextSchoolYear}"),
                sessionType = SessionType.schoolYear,
                schoolYear = schoolYear
            };
            this.academicSessions.Add(academicSession);

            // Marking Periods
            AcademicSession academicSessionMP1 = new AcademicSession
            {
                sourcedId = Guid.NewGuid(),
                Status = StatusType.active,
                title = $"MP1 {schoolYear}-{nextSchoolYear}",
                startDate = DateTime.Parse($"8/30/{schoolYear}"),
                endDate = DateTime.Parse($"11/09/{schoolYear}"),
                sessionType = SessionType.gradingPeriod,
                schoolYear = schoolYear
            };
            this.academicSessions.Add(academicSessionMP1);

            AcademicSession academicSessionMP2 = new AcademicSession
            {
                sourcedId = Guid.NewGuid(),
                Status = StatusType.active,
                title = $"MP2 {schoolYear}-{nextSchoolYear}",
                startDate = DateTime.Parse($"11/10/{schoolYear}"),
                endDate = DateTime.Parse($"01/29/{nextSchoolYear}"),
                sessionType = SessionType.gradingPeriod,
                schoolYear = schoolYear
            };
            this.academicSessions.Add(academicSessionMP2); 
            
            AcademicSession academicSessionMP3 = new AcademicSession
            {
                sourcedId = Guid.NewGuid(),
                Status = StatusType.active,
                title = $"MP3 {schoolYear}-{nextSchoolYear}",
                startDate = DateTime.Parse($"01/30/{nextSchoolYear}"),
                endDate = DateTime.Parse($"04/13/{nextSchoolYear}"),
                sessionType = SessionType.gradingPeriod,
                schoolYear = schoolYear
            };
            this.academicSessions.Add(academicSessionMP3);

            AcademicSession academicSessionMP4 = new AcademicSession
            {
                sourcedId = Guid.NewGuid(),
                Status = StatusType.active,
                title = $"MP4 {schoolYear}-{nextSchoolYear}",
                startDate = DateTime.Parse($"4/14/{nextSchoolYear}"),
                endDate = DateTime.Parse($"6/30/{nextSchoolYear}"),
                sessionType = SessionType.gradingPeriod,
                schoolYear = schoolYear
            };
            this.academicSessions.Add(academicSessionMP4);

            // Semesters

            AcademicSession academicSessionS1 = new AcademicSession
            {
                sourcedId = Guid.NewGuid(),
                Status = StatusType.active,
                title = $"S1 {schoolYear}-{nextSchoolYear}",
                startDate = DateTime.Parse($"8/30/{schoolYear}"),
                endDate = DateTime.Parse($"1/29/{nextSchoolYear}"),
                sessionType = SessionType.term,
                schoolYear = schoolYear
            };
            this.academicSessions.Add(academicSessionS1);

            AcademicSession academicSessionS2 = new AcademicSession
            {
                sourcedId = Guid.NewGuid(),
                Status = StatusType.active,
                title = $"S2 {schoolYear}-{nextSchoolYear}",
                startDate = DateTime.Parse($"1/30/{nextSchoolYear}"),
                endDate = DateTime.Parse($"6/30/{nextSchoolYear}"),
                sessionType = SessionType.term,
                schoolYear = schoolYear
            };
            this.academicSessions.Add(academicSessionS2);

        }
        #endregion

        #region "Teacher"
        /// <summary>
        /// Creates a Teacher Record
        /// </summary>
        /// <returns></returns>
        public Teacher CreateTeacher(Org org = null)
        {
            var maxTeachers = File.ReadAllLines(TEACHERS_FILE).Length - 1;
            var rnd = new Random();
            var rndLine = rnd.Next(0, maxTeachers);
            var teacherName = File.ReadLines(TEACHERS_FILE).Skip(rndLine).Take(1).First();
            var staffid = "00000000" + NUM_STAFF_ID.ToString();
            Teacher teacher = new Teacher
            {
                sourcedId = Guid.NewGuid(),
                identifier = staffid.Substring(staffid.Length - 8, 8),
                enabledUser = true,
                givenName = teacherName.Split(" ")[0],
                familyName = teacherName.Split(" ")[1],
                org = org
            };
            NUM_STAFF_ID++;
            teachers.Add(teacher);
            return teacher;
        }
        #endregion

        #region "Students"
        /// <summary>
        /// Start Student Generation
        /// </summary>
        void GenerateStudents()
        {
            var rnd = new Random();
            var maxFirstNames = File.ReadAllLines(STUDENT_FIRSTNAME_FILE).Length - 1;
            var maxLastNames = File.ReadAllLines(STUDENT_LASTNAME_FILE).Length - 1;
            foreach (Org org in orgs.Where(e => e.orgType == OrgType.school))
            {
                foreach (var grade in org.gradesOffer)
                {
                    for (var i = 1; i < NUM_STUDENTS_PER_GRADE; i++)
                    {
                        NUM_STUDENT_ID++;
                        var FName = rnd.Next(0, maxFirstNames);
                        var LName = rnd.Next(0, maxLastNames);
                        var stu = new Student
                        {
                            sourcedId = Guid.NewGuid(),
                            identifier = NUM_STUDENT_ID.ToString(),
                            enabledUser = true,
                            givenName = File.ReadLines(STUDENT_FIRSTNAME_FILE).Skip(FName).Take(1).First(),
                            familyName = File.ReadLines(STUDENT_LASTNAME_FILE).Skip(LName).Take(1).First(),
                            grade = grade,
                            org = org,
                            // Assign each student all courses of their current grade
                            courses = courses.Where(e => e.title.Contains(grade.name)).ToList()
                        };
                        students.Add(stu);
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
            if (GRADES == "ALL")
            {
                using (var reader = new StreamReader(GRADES_FILE))
                {
                    int gradeId = 1;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        Grade newGrade = new Grade
                        {
                            id = gradeId,
                            name = values[0]
                        };
                        gradeId++;
                        grades.Add(newGrade);
                    }
                }
            }
        }
        #endregion

        #region "Orgs"
        /// <summary>
        /// Generate Orgs
        /// </summary>
        void GenerateOrgs()
        {
            orgs.Add(parentOrg);

            var maxSchools = File.ReadAllLines(ORGS_FILE).Length - 1;
            var rnd = new Random();

            //TODO: Validate this is possible

            var randomSeq = Enumerable.Range(1, maxSchools).OrderBy(r => rnd.NextDouble()).Take(NUM_SCHOOLS).ToList();
            string[] schoolTypes = { "Elementary School", "Elementary School", "Middle School", "Middle School", "High School" };

            foreach (var schoolNum in randomSeq)
            {
                string line = File.ReadLines(ORGS_FILE).Skip(schoolNum).Take(1).First();
                var paddedOrgNum = ("0000" + schoolNum.ToString());
                Org newOrg = new Org
                {
                    sourcedId = Guid.NewGuid(),
                    identifier = paddedOrgNum.Substring(paddedOrgNum.Length - 4, 4),
                    name = $"{line} {schoolTypes[rnd.Next(schoolTypes.Length)]}",
                    parentSourcedId = parentOrg.sourcedId,
                    orgType = OrgType.school
                };
                if (newOrg.name.Contains("Elementary"))
                {
                    newOrg.gradesOffer = grades.Where(e => elemGrades.Contains(e.name)).ToList();
                }
                if (newOrg.name.Contains("Middle"))
                {
                    newOrg.gradesOffer = grades.Where(e => middleGrades.Contains(e.name)).ToList();
                }
                if (newOrg.name.Contains("High"))
                {
                    newOrg.gradesOffer = grades.Where(e => highGrades.Contains(e.name)).ToList();
                }
                orgs.Add(newOrg);
            }

        }
        #endregion

        #region "Courses"
        /// <summary>
        /// Start Course Generation
        /// </summary>
        void GenerateCourses()
        {
            using (var reader = new StreamReader(COURSES_FILE))
            {

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    var tmpGrade = values[1].ToString();
                    var grade = tmpGrade.Substring(tmpGrade.Length - 2, 2);
                    Course newCourse = new Course
                    {
                        sourcedId = Guid.NewGuid(),
                        title = values[1],
                        courseCode = values[0],
                        orgSourcedId = parentOrg.sourcedId,
                        schoolYearSourcedId = this.academicSessions.Where(e => e.title.Contains(values[2].ToString())).FirstOrDefault().sourcedId,
                        grade = grades.Where(e => e.name.Contains(grade)).First()
                    };
                    courses.Add(newCourse);
                }
            }
        }
        #endregion
    }
}
