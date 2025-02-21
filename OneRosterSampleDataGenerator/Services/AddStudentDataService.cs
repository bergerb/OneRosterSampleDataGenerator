﻿using OneRosterSampleDataGenerator.Helpers;
using OneRosterSampleDataGenerator.Models;
using System;
using System.Linq;

namespace OneRosterSampleDataGenerator.Services
{
    public class AddStudentDataService(
        DateTime dateLastModified,
        Students students,
        Enrollments enrollments,
        Orgs orgs,
        Courses courses,
        StatusChangeBuilder statusChangeBuilder)
    {
        private static readonly Random _random = new();

        private readonly Courses _courses = courses;
        private readonly DateTime _dateLastModified = dateLastModified;
        private readonly Enrollments _enrollments = enrollments;
        private readonly Orgs _orgs = orgs;
        private readonly StatusChangeBuilder _statusChangeBuilder = statusChangeBuilder;
        private readonly Students _students = students;

        public class DataContext
        {
            public Students Students { get; set; }
            public Enrollments Enrollments { get; set; }
        }


        public DataContext AddStudents(int maxRecordCount)
        {
            for (int i = 0; i <= _random.Next(0, maxRecordCount); i++)
            {
                this.AddRandomStudent();
            }

            return new()
            {
                Enrollments = _enrollments,
                Students = _students,
            };
        }

        void AddRandomStudent()
        {
            if (_orgs.Items.Count == 0 || _orgs.Items.Count == 0)
                return;
            try
            {
                var org = Utility.GetRandomItem(_orgs.Items.Where(x => x.OrgType == OrgType.school).ToList());
                var grade = Utility.GetRandomItem(org.GradesOffer);

                // Add student to the org
                var student = _students.AddStudent(org, grade, createdAt: _dateLastModified);

                _statusChangeBuilder.AddEvent(
                    StatusChangeBuilder.EventAction.Created,
                    StatusChangeBuilder.Type.Student,
                    $"{student.SourcedId} {student.FamilyName}, {student.GivenName} (Grade: {student.Grade.Name}) created at {org.Name}.");

                // Find a student in the same org and grade
                var existingStudent = _students.Items
                    .Where(x => x.Org.Id == org.Id)
                    .Where(x => x.Grade.Id == grade.Id)
                    .FirstOrDefault();

                // Add enrollments
                if (existingStudent is not null)
                {
                    // Find existing student's enrollments
                    var studentEnrollments = _enrollments.Items
                        .Where(x => x.UserSourcedId == existingStudent.SourcedId)
                        .ToList();

                    // Add the new student to the same classes
                    foreach (var enrollment in studentEnrollments)
                    {
                        EnrollStudent(student, enrollment);
                    }
                }

                #region Local Functions

                void EnrollStudent(User student, Enrollment enrollment)
                {
                    _enrollments.AddEnrollment(
                        student,
                        enrollment.ClassSourcedId,
                        enrollment.CourseSourcedId,
                        enrollment.SchoolSourcedId,
                        RoleType.student,
                        _dateLastModified);

                    var courseTitle = _courses.GetCourseTitle(enrollment.CourseSourcedId);

                    _statusChangeBuilder.AddEvent(
                        StatusChangeBuilder.EventAction.Created,
                        StatusChangeBuilder.Type.Enrollment,
                        $"{student.FamilyName}, {student.GivenName} (Grade: {student.Grade.Name}) enrolled into {courseTitle}.");
                }

                #endregion
            }
            catch (Exception ex)
            {
                return;
            }
        }

    }
}
