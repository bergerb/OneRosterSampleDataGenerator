using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Staff : BaseModel, IUser
    {
        public bool EnabledUser { get; set; }
        public DateTime DateLastModified => CreatedAt;
        public Guid SourcedId { get; set; }
        public List<Class> Classes = new List<Class>();
        public string CurrentOrgName => this.Org.Name;
        public string Email => UserName + "@domain.local";
        public string FamilyName { get; set; } = null!;
        public string GivenName { get; set; } = null!;
        public string Identifier { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public Org Org { get; set; } = null!;
        public RoleType RoleType { get; set; }

        public void AddClass(Class @class)
        {
            Classes.Add(@class);
        }
    }
}
