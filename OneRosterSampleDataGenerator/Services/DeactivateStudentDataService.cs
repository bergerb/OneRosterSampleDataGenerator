using OneRosterSampleDataGenerator.Extensions;
using OneRosterSampleDataGenerator.Helpers;
using OneRosterSampleDataGenerator.Models;
using OneRosterSampleDataGenerator.Models.Interfaces;
using System;
using System.Linq;

namespace OneRosterSampleDataGenerator.Services
{
    public record DeactivateStudentDataService
    {
        private readonly DateTime _dateLastModified;
        private readonly Students _students;
        private readonly Enrollments _enrollments;
        private readonly Courses _courses;
        private readonly StatusChangeBuilder _statusChangeBuilder;

        public DeactivateStudentDataService(
            DateTime dateLastModified,
            Students students,
            Enrollments enrollments,
            Courses courses,
            StatusChangeBuilder statusChangeBuilder)
        {
            this._dateLastModified = dateLastModified;
            this._enrollments = enrollments;
            this._students = students;
            this._courses = courses;
            this._statusChangeBuilder = statusChangeBuilder;
        }

        public class DataContext
        {
            public Students Students { get; set; }
            public Enrollments Enrollments { get; set; }
        }

        public DataContext DeactivateStudents(int maxRecordCount)
        {
            for (int j = 0; j <= new Random().Next(0, maxRecordCount); j++)
            {
                DeactivateRandomStudent();
            }

            return new()
            {
                Enrollments = _enrollments,
                Students = _students,
            };
        }

        private void DeactivateRandomStudent()
        {
            var randomStudent = new Random().Next(0, _students.Items.Count - 1);
            var student = _students.Items[randomStudent]
                .DeactivateUser(_dateLastModified);

            student.DeactivateUser(_dateLastModified);

            _statusChangeBuilder.AddEvent(
                StatusChangeBuilder.EventAction.Deactivated,
                StatusChangeBuilder.Type.Student,
                $"{student.SourcedId} {student.FamilyName}, {student.GivenName} (Grade: {student.Grade.Name}) modified at {student.Org.Name}.");

            DeactivateEnrollmentsForUser(student);
        }

        void DeactivateEnrollmentsForUser(ILeaUser user)
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
}
