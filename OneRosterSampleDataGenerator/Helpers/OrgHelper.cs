using OneRosterSampleDataGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Helpers;

public static class OrgHelper
{
    public static Org CreateSchool(string identifier, string name, DateTime dateLastModified, Guid parentOrgId, List<Grade> grades)
    {
        Org newOrg = new()
        {
            SourcedId = Guid.NewGuid(),
            DateLastModified = dateLastModified,
            Identifier = identifier,
            Name = name,
            ParentSourcedId = parentOrgId,
            OrgType = OrgType.school
        };
        if (newOrg.Name.Contains("Elementary"))
        {
            newOrg.GradesOffer = grades.Where(e => GradeHelper.Elementary.Contains(e.Name)).ToList();
        }
        if (newOrg.Name.Contains("Middle"))
        {
            newOrg.GradesOffer = grades.Where(e => GradeHelper.Middle.Contains(e.Name)).ToList();
        }
        if (newOrg.Name.Contains("High"))
        {
            newOrg.GradesOffer = grades.Where(e => GradeHelper.High.Contains(e.Name)).ToList();
        }
        return newOrg;
    }
}
