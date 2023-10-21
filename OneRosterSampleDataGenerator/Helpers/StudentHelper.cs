using OneRosterSampleDataGenerator.Models;
using System;

namespace OneRosterSampleDataGenerator.Helpers;

public class StudentHelper
{
    public static User DeactivateStudent(User student, DateTime dateLastModified)
    {
        student.Status = StatusType.inactive;
        student.DateLastModified = dateLastModified;

        return student;
    }
}
