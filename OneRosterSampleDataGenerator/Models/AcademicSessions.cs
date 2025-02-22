using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public class AcademicSessions(DateTime createdAt) : Generator<AcademicSession>(createdAt)
{
    public override List<AcademicSession> Generate()
    {
        this.Items = this.CreateSessions().ToList();

        return this.Items;
    }

    private IEnumerable<AcademicSession> CreateSessions()
    {
        var schoolYear = Utility.GetCurrentSchoolYear();
        var nextSchoolYear = Utility.GetNextSchoolYear();

        // Create SchoolYear Term
        AcademicSession academicSession = new()
        {
            DateLastModified = this.CreatedAt,
            EndDate = DateTime.Parse($"8/15/{nextSchoolYear}"),
            SchoolYear = schoolYear,
            SessionType = SessionType.schoolYear,
            SourcedId = Guid.NewGuid(),
            StartDate = DateTime.Parse($"8/16/{schoolYear}"),
            Status = StatusType.active,
            Title = $"FY {schoolYear}-{nextSchoolYear}",
        };
        yield return academicSession;

        // Marking Periods
        AcademicSession academicSessionMP1 = new()
        {
            DateLastModified = this.CreatedAt,
            EndDate = DateTime.Parse($"11/09/{schoolYear}"),
            SchoolYear = schoolYear,
            SessionType = SessionType.gradingPeriod,
            SourcedId = Guid.NewGuid(),
            StartDate = DateTime.Parse($"8/30/{schoolYear}"),
            Status = StatusType.active,
            Title = $"MP1 {schoolYear}-{nextSchoolYear}",
        };
        yield return academicSessionMP1;

        AcademicSession academicSessionMP2 = new()
        {
            DateLastModified = this.CreatedAt,
            EndDate = DateTime.Parse($"01/29/{nextSchoolYear}"),
            SchoolYear = schoolYear,
            SessionType = SessionType.gradingPeriod,
            SourcedId = Guid.NewGuid(),
            StartDate = DateTime.Parse($"11/10/{schoolYear}"),
            Status = StatusType.active,
            Title = $"MP2 {schoolYear}-{nextSchoolYear}",
        };
        yield return academicSessionMP2;

        AcademicSession academicSessionMP3 = new()
        {
            DateLastModified = this.CreatedAt,
            EndDate = DateTime.Parse($"04/13/{nextSchoolYear}"),
            SchoolYear = schoolYear,
            SessionType = SessionType.gradingPeriod,
            SourcedId = Guid.NewGuid(),
            StartDate = DateTime.Parse($"01/30/{nextSchoolYear}"),
            Status = StatusType.active,
            Title = $"MP3 {schoolYear}-{nextSchoolYear}",
        };
        yield return academicSessionMP3;

        AcademicSession academicSessionMP4 = new()
        {
            DateLastModified = this.CreatedAt,
            EndDate = DateTime.Parse($"6/30/{nextSchoolYear}"),
            SchoolYear = schoolYear,
            SessionType = SessionType.gradingPeriod,
            SourcedId = Guid.NewGuid(),
            StartDate = DateTime.Parse($"4/14/{nextSchoolYear}"),
            Status = StatusType.active,
            Title = $"MP4 {schoolYear}-{nextSchoolYear}",
        };
        yield return academicSessionMP4;

        // Semesters

        AcademicSession academicSessionS1 = new()
        {
            DateLastModified = this.CreatedAt,
            EndDate = DateTime.Parse($"1/29/{nextSchoolYear}"),
            SchoolYear = schoolYear,
            SessionType = SessionType.term,
            SourcedId = Guid.NewGuid(),
            StartDate = DateTime.Parse($"8/30/{schoolYear}"),
            Status = StatusType.active,
            Title = $"S1 {schoolYear}-{nextSchoolYear}",
        };
        yield return academicSessionS1;

        AcademicSession academicSessionS2 = new()
        {
            DateLastModified = this.CreatedAt,
            EndDate = DateTime.Parse($"6/30/{nextSchoolYear}"),
            SchoolYear = schoolYear,
            SessionType = SessionType.term,
            SourcedId = Guid.NewGuid(),
            StartDate = DateTime.Parse($"1/30/{nextSchoolYear}"),
            Status = StatusType.active,
            Title = $"S2 {schoolYear}-{nextSchoolYear}",
        };
        yield return academicSessionS2;

        AcademicSession academicSessionSummer = new()
        {
            DateLastModified = this.CreatedAt,
            EndDate = DateTime.Parse($"8/15/{nextSchoolYear}"),
            SchoolYear = schoolYear,
            SessionType = SessionType.semester,
            SourcedId = Guid.NewGuid(),
            StartDate = DateTime.Parse($"6/30/{nextSchoolYear}"),
            Status = StatusType.active,
            Title = $"Summer {schoolYear}-{nextSchoolYear}",
        };
        yield return academicSessionSummer;
    }
}
