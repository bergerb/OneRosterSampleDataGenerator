using OneRosterSampleDataGenerator.Models;
using System;

namespace OneRosterSampleDataGenerator.Helpers;

public class StudentHelper
{
    public static Student DeactivateStudent(Student student, DateTime dateLastModified)
    {
        student.Status = StatusType.inactive;
        student.DateLastModified = dateLastModified;

        return student;
    }
}
