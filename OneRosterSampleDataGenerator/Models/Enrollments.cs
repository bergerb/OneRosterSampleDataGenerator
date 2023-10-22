using OneRosterSampleDataGenerator.Models.Base;
using OneRosterSampleDataGenerator.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class Enrollments : Generator<Enrollment>
{
    public Enrollments(DateTime createdAt)
        : base(createdAt)
    {
    }

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
        ILeaUser user,
        Guid classSourcedId,
        Guid courseSourcedId,
        Guid schoolSourcedId,
        RoleType role,
        DateTime? createdAt = null)
    {
        Enrollment enrollment = new()
        {
            ClassSourcedId = classSourcedId,
            CourseSourcedId = courseSourcedId,
            DateLastModified = CreatedAt,
            RoleType = role,
            SchoolSourcedId = schoolSourcedId,
            SourcedId = Guid.NewGuid(),
            Status = StatusType.active,
            UserSourcedId = user.SourcedId,
        };

        if (createdAt.HasValue)
        {
            enrollment.DateLastModified = createdAt.Value;
        }

        AddItem(enrollment);

        return enrollment;
    }
    public Enrollment InactivateEnrollment(Guid enrollmentId, DateTime dateLastModified)
    {
        Enrollment enrollment = Items.FirstOrDefault(x => x.SourcedId == enrollmentId)
            ?? throw new Exception($"Enrollment with sourcedId {enrollmentId} not found");

        enrollment.Status = StatusType.tobedeleted;
        enrollment.DateLastModified = dateLastModified;

        return enrollment;
    }
}
