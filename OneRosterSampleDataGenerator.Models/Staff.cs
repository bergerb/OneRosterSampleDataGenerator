using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Staff : BaseModel, IUser
    {
        public Guid SourcedId { get; set; }
        public string Identifier { get; set; } = null!;
        public bool EnabledUser { get; set; }
        public string GivenName { get; set; } = null!;
        public string FamilyName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email => UserName + "@domain.local";

        public string CurrentOrgName => this.Org.Name;

        public Org Org { get; set; } = null!;
        public RoleType RoleType { get; set; }

        public List<Class> Classes = new List<Class>();
        public void AddClass(Class @class)
        {
            Classes.Add(@class);
        }
    }

}
