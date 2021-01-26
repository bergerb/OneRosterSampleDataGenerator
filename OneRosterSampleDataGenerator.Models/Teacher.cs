﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Teacher : IUser
    {
        public Guid id { get; set; }
        public string identifier { get; set; }
        public bool enabledUser { get; set; }
        public string givenName { get; set; }
        public string familyName { get; set; }

        public string userName => givenName.Substring(0, 1) + familyName;

        public string email => this.userName + "@domain.local";

        public string currentOrgName => this.org.name;

        public Org org { get;set; }
    }
}
