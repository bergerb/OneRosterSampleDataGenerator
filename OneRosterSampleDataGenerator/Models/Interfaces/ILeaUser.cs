using System;
using System.Collections.Generic;

namespace OneRosterSampleDataGenerator.Models.Interfaces
{
    public interface ILeaUser
    {
        List<Course> Courses { get; set; }
        string CurrentGrade { get; }
        string CurrentOrgName { get; }
        DateTime DateLastModified { get; set; }
        string Email { get; }
        bool EnabledUser { get; set; }
        string FamilyName { get; set; }
        string GivenName { get; set; }
        Grade? Grade { get; set; }
        string Identifier { get; set; }
        Org Org { get; set; }
        RoleType RoleType { get; set; }
        Guid SourcedId { get; set; }
        string UserName { get; set; }
        List<Class> Classes { get; set; }
        void AddClass(Class @class);
    }
}