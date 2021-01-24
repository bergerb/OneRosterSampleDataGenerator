using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Student
    {
        public Guid id { get; set; }
        public int identifier { get; set; }
        //FirstName
        public string givenName { get; set; }
        public string familyName { get; set; }
        public string userName => givenName.Substring(0, 1) + familyName + identifier.ToString().Substring(6, 3);
        public string email => userName  + "@domain.local";
        public string currentGrade => this.grade.name;
        public Grade grade { get; set; }
        public string currentOrgName => this.org.name;
        public Org org { get; set; }
    }
}
