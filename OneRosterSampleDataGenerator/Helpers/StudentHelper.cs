using OneRosterSampleDataGenerator.Models;
using System;

namespace OneRosterSampleDataGenerator.Helpers;

public class StudentHelper
{
    public static Student DeactivateStudent(Student student)
    {
        student.Status = StatusType.inactive;
        student.DateLastModified = DateTime.Now;

        return student;
    }
}
