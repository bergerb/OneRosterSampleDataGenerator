using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public record Enrollments(
    DateTime createdAt) : Generator<Enrollment>
{
    public override List<Enrollment> Generate()
    {
        Items = CreateEnrollments().ToList();

        return Items.ToList();
    }

    private IEnumerable<Enrollment> CreateEnrollments()
    {
        return Items;
    }

    public Enrollment AddEnrollment(
        IUser user,
        Guid classSourcedId,
        Guid courseSourcedId,
        Guid schoolSourcedId,
        RoleType role)
    {
        Enrollment enrollment = new()
        {
            DateLastModified = createdAt,
            ClassSourcedId = classSourcedId,
            CourseSourcedId = courseSourcedId,
            SchoolSourcedId = schoolSourcedId,
            SourcedId = Guid.NewGuid(),
            Status = StatusType.active,
            UserSourcedId = user.SourcedId,
            RoleType = role
        };

        AddItem(enrollment);

        return enrollment;
    }
}
