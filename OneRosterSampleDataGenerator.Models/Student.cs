using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Student
    {
        public Guid id { get; set; }
        public int studentId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string currentGrade => this.grade.name;
        public Grade grade { get; set; }
        public string currentOrgName => this.org.name; 
        public Org org { get; set; }
    }
}
