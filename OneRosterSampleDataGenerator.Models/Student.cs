﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Student : BaseModel, IUser
    {
        public Guid SourcedId { get; set; }
        public string Identifier { get; set; }
        public bool EnabledUser { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string UserName => GivenName.Substring(0, 1) + FamilyName + Identifier.Substring(6, 3);
        public string Email => this.UserName + "@domain.local";
        public string CurrentOrgName => this.Org.Name;
        public Org Org { get; set; }
        public string CurrentGrade => this.Grade.Name;
        public Grade Grade { get; set; }
        public List<Course> Courses { get; set; }
        public RoleType RoleType { get => RoleType.student; set => throw new NotImplementedException(); }
    }
}
