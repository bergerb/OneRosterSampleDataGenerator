using OneRosterSampleDataGenerator.Models;
using System;

namespace OneRosterSampleDataGenerator.Helpers;

public class EnrollmentHelper
{
    public static Enrollment InactivateEnrollment(Enrollment enrollment, DateTime dateLastModified)
    {
        enrollment.Status = StatusType.inactive;
        enrollment.DateLastModified = dateLastModified;

        return enrollment;
    }
}
