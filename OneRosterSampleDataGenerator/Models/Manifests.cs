using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class Manifests : Generator<Manifest>
{
    public Manifests(DateTime createdAt, Org parentOrg) : base(createdAt)
    {
        ParentOrg = parentOrg;
    }

    public Org ParentOrg { get; set; }
    public override List<Manifest> Generate()
    {
        Items = CreateManifests().ToList();

        return Items;
    }

    private List<Manifest> CreateManifests()
    {
        var items = new List<Manifest>() {
            new Manifest() { PropertyName = "propertyName", Value = "value" },
            new Manifest() { PropertyName = "manifest.version", Value = "1.0" },
            new Manifest() { PropertyName = "oneroster.version", Value = "1.1" },
            new Manifest() { PropertyName = "source.systemName", Value = ParentOrg.Name + " OneRoster" },
            new Manifest() { PropertyName = "source.systemCode", Value = ParentOrg.Identifier },
            new Manifest() { PropertyName = "file.academicSessions", Value = "bulk" },
            new Manifest() { PropertyName = "file.orgs", Value = "bulk" },
            new Manifest() { PropertyName = "file.courses", Value = "bulk" },
            new Manifest() { PropertyName = "file.classes", Value = "bulk" },
            new Manifest() { PropertyName = "file.users", Value = "bulk" },
            new Manifest() { PropertyName = "file.enrollments", Value = "bulk" },
            new Manifest() { PropertyName = "file.demographics", Value = "bulk" },
            new Manifest() { PropertyName = "file.resources", Value = "absent" },
            new Manifest() { PropertyName = "file.classResources", Value = "absent" },
            new Manifest() { PropertyName = "file.courseResources", Value = "absent" },
            new Manifest() { PropertyName = "file.categories", Value = "absent" },
            new Manifest() { PropertyName = "file.lineItems", Value = "absent" },
            new Manifest() { PropertyName = "file.results", Value = "absent" },
            };

        return items;
    }
}
