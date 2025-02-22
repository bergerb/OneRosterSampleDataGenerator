using OneRosterSampleDataGenerator.Models.Base;
using OneRosterSampleDataGenerator.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class Enrollments(DateTime createdAt) : Generator<Enrollment>(createdAt)
{
    public override List<Enrollment> Generate()
    {
        this.Items = this.CreateEnrollments();

        return [.. this.Items];
    }

    private List<Enrollment> CreateEnrollments()
    {
        return this.Items;
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
            DateLastModified = this.CreatedAt,
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

        this.AddItem(enrollment);

        return enrollment;
    }

    public Enrollment InactivateEnrollment(Guid enrollmentId, DateTime dateLastModified)
    {
        Enrollment enrollment = this.Items.FirstOrDefault(x => x.SourcedId == enrollmentId)
            ?? throw new Exception($"Enrollment with sourcedId {enrollmentId} not found");

        enrollment.Status = StatusType.tobedeleted;
        enrollment.DateLastModified = dateLastModified;

        return enrollment;
    }
}
