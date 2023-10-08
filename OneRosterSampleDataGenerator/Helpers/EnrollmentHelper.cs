using OneRosterSampleDataGenerator.Models;
using System;

namespace OneRosterSampleDataGenerator.Helpers;

public class EnrollmentHelper
{
    public static Enrollment InactivateEnrollment(Enrollment enrollment)
    {
        enrollment.Status = StatusType.inactive;
        enrollment.DateLastModified = DateTime.Now;

        return enrollment;
    }
}
