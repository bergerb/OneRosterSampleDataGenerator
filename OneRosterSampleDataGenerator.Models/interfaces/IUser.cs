using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public interface IUser
    {
        Guid id { get; set; }
        bool enabledUser { get; set; }
        string identifier { get; set; }
        //FirstName
        string givenName { get; set; }
        string familyName { get; set; }
        string userName { get; }
        string email { get; }

        string currentOrgName { get; }
        Org org { get; set; }
    }
}
