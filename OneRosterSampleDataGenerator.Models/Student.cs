using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Student : BaseModel, IUser
    {
        public Guid SourcedId { get; set; }
        public string Identifier { get; set; } = null!;
        public bool EnabledUser { get; set; }
        public string GivenName { get; set; } = null!;
        public string FamilyName { get; set; } = null!;
        public string UserName => GivenName.Substring(0, 1) + FamilyName + Identifier.Substring(6, 3);
        public string Email => this.UserName + "@domain.local";
        public string CurrentOrgName => this.Org.Name;
        public Org Org { get; set; } = null!;
        public string CurrentGrade => this.Grade.Name;
        public Grade Grade { get; set; } = null!;
        public List<Course> Courses { get; set; } = new List<Course>();
        public RoleType RoleType { get => RoleType.student; set => throw new NotImplementedException(); }
    }
}
