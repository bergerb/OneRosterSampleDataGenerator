using OneRosterSampleDataGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OneRosterSampleDataGenerator
{
    public class OneRoster
    {
        int NUM_SCHOOLS = 10;
        string GRADES = "ALL";
        public List<Grade> grades = new List<Grade>();
        public List<Building> buildings = new List<Building>();

        Assembly assembly = Assembly.GetExecutingAssembly();
        const string GRADES_FILE = @"../../../../Templates/planets/grades.csv";
        const string ORGS_FILE = @"../../../../Templates/planets/orgs.csv";

        string[] elemGrades = "KG,01,02,03,04,05".Split(',');
        string[] middleGrades = "06,07,08".Split(',');
        string[] highGrades = "09,10,11,12".Split(',');

        /// <summary>
        /// Instantiate OneRoster 
        /// </summary>
        public OneRoster()
        {
            // Build Grades
            GenerateGrades();
            // Build Schools
            GenerateOrgs();
        }

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
                        var values = line.Split(';');
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
            var maxSchools = File.ReadAllLines(ORGS_FILE).Length;
            var rnd = new Random();
            var randomSeq = Enumerable.Range(1, maxSchools).OrderBy(r => rnd.NextDouble()).Take(NUM_SCHOOLS).ToList();
            string[] schoolTypes = { "Elementary School", "Middle School", "High School" };

            foreach (var schoolNum in randomSeq)
            {
                string line = File.ReadLines(ORGS_FILE).Skip(schoolNum).Take(1).First();
                var paddedSchoolNum = ("00000" + schoolNum.ToString());
                Building newBuilding = new Building
                {
                    id = Guid.NewGuid(),
                    number = paddedSchoolNum.Substring(paddedSchoolNum.Length - 4, 4),
                    name = $"{line} {schoolTypes[rnd.Next(schoolTypes.Length)]}",
                };
                if (newBuilding.name.Contains("Elementary"))
                {
                    newBuilding.gradesOffer = grades.Where(e => elemGrades.Contains(e.name)).ToList();
                }
                if (newBuilding.name.Contains("Middle"))
                {
                    newBuilding.gradesOffer = grades.Where(e => middleGrades.Contains(e.name)).ToList();
                }
                if (newBuilding.name.Contains("High"))
                {
                    newBuilding.gradesOffer = grades.Where(e => highGrades.Contains(e.name)).ToList();
                }
                buildings.Add(newBuilding);
            }
        }
        #endregion

    }
}
