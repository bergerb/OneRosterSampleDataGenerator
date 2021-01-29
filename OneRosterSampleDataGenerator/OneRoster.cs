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

        int NUM_STUDENT_ID = 910000000;

        int NUM_STAFF_ID = 1;

        string CURRENT_YEAR = "";

        string GRADES = "ALL";
        public List<AcademicSession> academicSessions = new List<AcademicSession>();
        public List<Grade> grades = new List<Grade>();
        public List<Org> orgs = new List<Org>();
        public List<Course> courses = new List<Course>();
        public List<Student> students = new List<Student>();
        public List<Teacher> teachers = new List<Teacher>();

        Org parentOrg = new Org
        {
            id = Guid.NewGuid(),
            name = "Test District",
            number = "9999"
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
        }
        #region "Academic Sessions"

        private void GenerateAcademicSessions()
        {
            // Get Current School Year
            var schoolYear = Utility.GetCurrentSchoolYear();
            var nextSchoolYear = Utility.GetNextSchoolYear();
            // Create SchoolYear Term
            AcademicSession academicSession = new AcademicSession
            {
                sourcedId = Guid.NewGuid(),
                status = "active",
                dateLastModified = DateTime.Now,
                title = $"FY {schoolYear} - {nextSchoolYear}",
                startDate = $"7/1/{schoolYear}",
                endDate = $"6/30/{nextSchoolYear}",
                type = "schoolYear",
                schoolYear = schoolYear
            };
            this.academicSessions.Add(academicSession);

            AcademicSession academicSessionS1 = new AcademicSession
            {
                sourcedId = Guid.NewGuid(),
                status = "active",
                dateLastModified = DateTime.Now,
                title = $"S1 {schoolYear} - {nextSchoolYear}",
                startDate = $"7/1/{schoolYear}",
                endDate = $"1/15/{nextSchoolYear}",
                type = "schoolYear",
                schoolYear = schoolYear
            };
            this.academicSessions.Add(academicSessionS1);

            AcademicSession academicSessionS2 = new AcademicSession
            {
                sourcedId = Guid.NewGuid(),
                status = "active",
                dateLastModified = DateTime.Now,
                title = $"S2 {schoolYear} - {nextSchoolYear}",
                startDate = $"1/16/{nextSchoolYear}",
                endDate = $"6/30/{nextSchoolYear}",
                type = "schoolYear",
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
        public Teacher CreateTeacher(Org? org = null)
        {
            var maxTeachers = File.ReadAllLines(TEACHERS_FILE).Length - 1;
            var rnd = new Random();
            var rndLine = rnd.Next(0, maxTeachers);
            var teacherName = File.ReadLines(TEACHERS_FILE).Skip(rndLine).Take(1).First();
            var staffid = "00000000" + NUM_STAFF_ID.ToString();
            Teacher teacher = new Teacher
            {
                id = Guid.NewGuid(),
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
                            id = Guid.NewGuid(),
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
        /// Generate Gradess
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
                    id = Guid.NewGuid(),
                    number = paddedOrgNum.Substring(paddedOrgNum.Length - 4, 4),
                    name = $"{line} {schoolTypes[rnd.Next(schoolTypes.Length)]}",
                    parentOrgId = parentOrg.id
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
        void GenerateCourses()
        {
            using (var reader = new StreamReader(COURSES_FILE))
            {

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    Course newCourse = new Course
                    {
                        id = Guid.NewGuid(),
                        title = values[1],
                        courseCode = values[0],
                        orgSourcedId = parentOrg.id,
                        academicSessionId = this.academicSessions.Where(e => e.title.Contains(values[2].ToString())).FirstOrDefault().sourcedId

                    };
                    courses.Add(newCourse);
                }
            }
        }

        #endregion
    }
}
