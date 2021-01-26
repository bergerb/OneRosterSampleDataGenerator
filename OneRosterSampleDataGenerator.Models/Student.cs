using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Student : IUser
    {
        public Guid id { get; set; }
        public int identifier { get; set; }
        public string givenName { get; set; }
        public string familyName { get; set; }
        public string userName => givenName.Substring(0, 1) + familyName + identifier.ToString().Substring(6, 3);
        public string email => this.userName + "@domain.local";
        public string currentOrgName => this.org.name;
        public Org org { get; set; }
        public string currentGrade => this.grade.name;
        public Grade grade { get; set; }
        public List<Course> courses { get; set; }

    }
}
