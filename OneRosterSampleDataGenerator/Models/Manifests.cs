using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;

namespace OneRosterSampleDataGenerator.Models;

public class Manifests(DateTime createdAt, Org parentOrg) : Generator<Manifest>(createdAt)
{
    public Org ParentOrg { get; set; } = parentOrg;

    public override List<Manifest> Generate()
    {
        this.Items = this.CreateManifests();

        return this.Items;
    }

    private List<Manifest> CreateManifests()
    {
        var items = new List<Manifest>() {
            new() { PropertyName = "propertyName", Value = "value" },
            new() { PropertyName = "manifest.version", Value = "1.0" },
            new() { PropertyName = "oneroster.version", Value = "1.1" },
            new() { PropertyName = "source.systemName", Value = this.ParentOrg.Name + " OneRoster" },
            new() { PropertyName = "source.systemCode", Value = this.ParentOrg.Identifier },
            new() { PropertyName = "file.academicSessions", Value = "bulk" },
            new() { PropertyName = "file.orgs", Value = "bulk" },
            new() { PropertyName = "file.courses", Value = "bulk" },
            new() { PropertyName = "file.classes", Value = "bulk" },
            new() { PropertyName = "file.users", Value = "bulk" },
            new() { PropertyName = "file.enrollments", Value = "bulk" },
            new() { PropertyName = "file.demographics", Value = "bulk" },
            new() { PropertyName = "file.resources", Value = "absent" },
            new() { PropertyName = "file.classResources", Value = "absent" },
            new() { PropertyName = "file.courseResources", Value = "absent" },
            new() { PropertyName = "file.categories", Value = "absent" },
            new() { PropertyName = "file.lineItems", Value = "absent" },
            new() { PropertyName = "file.results", Value = "absent" },
            };

        return items;
    }
}
