using OneRosterSampleDataGenerator.Extensions;
using OneRosterSampleDataGenerator.Helpers;
using OneRosterSampleDataGenerator.Models;
using System;
using System.Linq;

namespace OneRosterSampleDataGenerator.Services;

public class DeactivateStudentDataService(
    DateTime dateLastModified,
    Students students,
    Enrollments enrollments,
    Courses courses,
    StatusChangeBuilder statusChangeBuilder)
{
    private static readonly Random _random = new();

    private readonly Courses _courses = courses;
    private readonly DateTime _dateLastModified = dateLastModified;
    private readonly Enrollments _enrollments = enrollments;
    private readonly StatusChangeBuilder _statusChangeBuilder = statusChangeBuilder;
    private readonly Students _students = students;

    public class DataContext
    {
        public Students Students { get; set; }
        public Enrollments Enrollments { get; set; }
    }

    public DataContext DeactivateStudents(int maxRecordCount)
    {
        var numStudents = _random.Next(0, maxRecordCount);

        for (int j = 0; j <= numStudents; j++)
        {
            this.DeactivateRandomStudent();
        }

        return new()
        {
            Enrollments = _enrollments,
            Students = _students,
        };
    }

    private void DeactivateRandomStudent()
    {
        Random rnd = new();
        if (_students.Items.Count == 0)
            return;

        var randomStudent = rnd.Next(0, _students.Items.Count - 1);

        var student = _students.Items[randomStudent]
            .DeactivateUser(_dateLastModified);

        student.DeactivateUser(_dateLastModified);

        _statusChangeBuilder.AddEvent(
            StatusChangeBuilder.EventAction.Deactivated,
            StatusChangeBuilder.Type.Student,
            $"{student.SourcedId} {student.FamilyName}, {student.GivenName} (Grade: {student.Grade?.Name}) modified at {student.Org.Name}.");

        this.DeactivateEnrollmentsForUser(student);
    }

    void DeactivateEnrollmentsForUser(User user)
    {
        var studentEnrollments = _enrollments.Items
            .Where(x => x.UserSourcedId == user.SourcedId)
            .ToList();
        foreach (var enrollment in studentEnrollments)
        {
            _enrollments.InactivateEnrollment(enrollment.SourcedId, _dateLastModified);

            var courseTitle = _courses.GetCourseTitle(enrollment.CourseSourcedId);
            _statusChangeBuilder.AddEvent(
                StatusChangeBuilder.EventAction.Deactivated,
                StatusChangeBuilder.Type.Enrollment,
                $"{user.FamilyName}, {user.GivenName} enrollment has been deactivated for {courseTitle}.");
        }
    }
}
