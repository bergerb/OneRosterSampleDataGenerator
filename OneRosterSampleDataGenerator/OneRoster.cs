using OneRosterSampleDataGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
        int NUM_SCHOOLS = 20;
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

        Org parentOrg = new Org
        {
            sourcedId = Guid.NewGuid(),
            name = "Test District",
            identifier = "9999",
            orgType = OrgType.district
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
        }

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
            foreach (Org org in orgs)
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
                title = $"FY {schoolYear} - {nextSchoolYear}",
                startDate = $"7/1/{schoolYear}",
                endDate = $"6/30/{nextSchoolYear}",
                sessionType = SessionType.schoolYear,
                schoolYear = schoolYear
            };
            this.academicSessions.Add(academicSession);

            AcademicSession academicSessionS1 = new AcademicSession
            {
                sourcedId = Guid.NewGuid(),
                Status = StatusType.active,
                title = $"S1 {schoolYear} - {nextSchoolYear}",
                startDate = $"7/1/{schoolYear}",
                endDate = $"1/15/{nextSchoolYear}",
                sessionType = SessionType.schoolYear,
                schoolYear = schoolYear
            };
            this.academicSessions.Add(academicSessionS1);

            AcademicSession academicSessionS2 = new AcademicSession
            {
                sourcedId = Guid.NewGuid(),
                Status = StatusType.active,
                title = $"S2 {schoolYear} - {nextSchoolYear}",
                startDate = $"1/16/{nextSchoolYear}",
                endDate = $"6/30/{nextSchoolYear}",
                sessionType = SessionType.schoolYear,
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
            foreach (Org org in orgs)
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
            var maxSchools = File.ReadAllLines(ORGS_FILE).Length - 1;
            var rnd = new Random();

            //TODO: Validate this is possible

            var randomSeq = Enumerable.Range(1, maxSchools).OrderBy(r => rnd.NextDouble()).Take(NUM_SCHOOLS).ToList();
            string[] schoolTypes = { "Elementary School", "Middle School", "High School" };

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
