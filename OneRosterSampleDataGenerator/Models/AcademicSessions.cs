using OneRosterSampleDataGenerator.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneRosterSampleDataGenerator.Models;

public record AcademicSessions(DateTime createdAt) : Generator<AcademicSession>
{
    public override List<AcademicSession> Generate()
    {
        Items = CreateSessions().ToList();

        return Items;
    }

    private IEnumerable<AcademicSession> CreateSessions()
    {
        var schoolYear = Utility.GetCurrentSchoolYear();
        var nextSchoolYear = Utility.GetNextSchoolYear();
        // Create SchoolYear Term
        AcademicSession academicSession = new()
        {
            SourcedId = Guid.NewGuid(),
            DateLastModified = createdAt,
            Status = StatusType.active,
            Title = $"FY {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"8/16/{schoolYear}"),
            EndDate = DateTime.Parse($"8/15/{nextSchoolYear}"),
            SessionType = SessionType.schoolYear,
            SchoolYear = schoolYear
        };
        yield return academicSession;

        // Marking Periods
        AcademicSession academicSessionMP1 = new()
        {
            SourcedId = Guid.NewGuid(),
            DateLastModified = createdAt,
            Status = StatusType.active,
            Title = $"MP1 {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"8/30/{schoolYear}"),
            EndDate = DateTime.Parse($"11/09/{schoolYear}"),
            SessionType = SessionType.gradingPeriod,
            SchoolYear = schoolYear
        };
        yield return academicSessionMP1;

        AcademicSession academicSessionMP2 = new()
        {
            SourcedId = Guid.NewGuid(),
            DateLastModified = createdAt,
            Status = StatusType.active,
            Title = $"MP2 {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"11/10/{schoolYear}"),
            EndDate = DateTime.Parse($"01/29/{nextSchoolYear}"),
            SessionType = SessionType.gradingPeriod,
            SchoolYear = schoolYear
        };
        yield return academicSessionMP2;

        AcademicSession academicSessionMP3 = new()
        {
            SourcedId = Guid.NewGuid(),
            DateLastModified = createdAt,
            Status = StatusType.active,
            Title = $"MP3 {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"01/30/{nextSchoolYear}"),
            EndDate = DateTime.Parse($"04/13/{nextSchoolYear}"),
            SessionType = SessionType.gradingPeriod,
            SchoolYear = schoolYear
        };
        yield return academicSessionMP3;

        AcademicSession academicSessionMP4 = new()
        {
            SourcedId = Guid.NewGuid(),
            DateLastModified = createdAt,
            Status = StatusType.active,
            Title = $"MP4 {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"4/14/{nextSchoolYear}"),
            EndDate = DateTime.Parse($"6/30/{nextSchoolYear}"),
            SessionType = SessionType.gradingPeriod,
            SchoolYear = schoolYear
        };
        yield return academicSessionMP4;

        // Semesters

        AcademicSession academicSessionS1 = new()
        {
            SourcedId = Guid.NewGuid(),
            DateLastModified = createdAt,
            Status = StatusType.active,
            Title = $"S1 {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"8/30/{schoolYear}"),
            EndDate = DateTime.Parse($"1/29/{nextSchoolYear}"),
            SessionType = SessionType.term,
            SchoolYear = schoolYear
        };
        yield return academicSessionS1;

        AcademicSession academicSessionS2 = new()
        {
            SourcedId = Guid.NewGuid(),
            DateLastModified = createdAt,
            Status = StatusType.active,
            Title = $"S2 {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"1/30/{nextSchoolYear}"),
            EndDate = DateTime.Parse($"6/30/{nextSchoolYear}"),
            SessionType = SessionType.term,
            SchoolYear = schoolYear
        };
        yield return academicSessionS2;

        AcademicSession academicSessionSummer = new()
        {
            SourcedId = Guid.NewGuid(),
            DateLastModified = createdAt,
            Status = StatusType.active,
            Title = $"Summer {schoolYear}-{nextSchoolYear}",
            StartDate = DateTime.Parse($"6/30/{nextSchoolYear}"),
            EndDate = DateTime.Parse($"8/15/{nextSchoolYear}"),
            SessionType = SessionType.semester,
            SchoolYear = schoolYear
        };
        yield return academicSessionSummer;
    }
}
