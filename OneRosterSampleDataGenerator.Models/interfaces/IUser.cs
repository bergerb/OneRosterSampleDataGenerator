using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public interface IUser
    {
        Guid SourcedId { get; set; }
        bool EnabledUser { get; set; }
        string Identifier { get; set; }
        //FirstName
        string GivenName { get; set; }
        string FamilyName { get; set; }
        string UserName { get; }
        string Email { get; }

        string CurrentOrgName { get; }
        Org Org { get; set; }

        RoleType RoleType { get; set; }
        public string Role => this.RoleType.ToString();
    }
}
