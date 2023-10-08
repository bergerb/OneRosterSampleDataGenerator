using NUnit.Framework;
using OneRosterSampleDataGenerator;
using Shouldly;
using System;

namespace Tests;

/// <summary>
/// Testing Academic Sessions
/// </summary>
public class AcademicSessionTests : RosterTest
{
    [Test]
    public void AcademicSessions_ShouldHaveRecords_WhenGenerated()
    {
        OneRoster.AcademicSessions.Count.ShouldBeGreaterThan(0);
    }

    [Test]
    public void SchoolYear_ShouldBeValid_ForAllSessions()
    {
        OneRoster.AcademicSessions
            .ForEach(session =>
            {
                session.SchoolYear.ShouldBe(Utility.GetCurrentSchoolYear());
            });
    }

    [TestCase("8/16/2019")]
    [TestCase("9/1/2020")]
    [TestCase("10/16/2021")]
    [TestCase("11/1/2022")]
    [TestCase("12/16/2023")]
    public void GetCurrentSchoolYear_ShouldCalculateCurrenyYear_WhenGivenDates(DateTime date)
    {
        Utility.GetCurrentSchoolYear(date).ShouldBe(date.Year.ToString());
    }

    [TestCase("1/1/2015")]
    [TestCase("2/15/2016")]
    [TestCase("3/1/2017")]
    [TestCase("4/15/2018")]
    [TestCase("5/1/2019")]
    [TestCase("6/15/2020")]
    [TestCase("7/1/2021")]
    [TestCase("8/1/2022")]
    [TestCase("8/14/2023")]
    public void GetCurrentSchoolYear_ShouldCalculatePreviousYear_WhenGivenDates(DateTime date)
    {
        Utility.GetCurrentSchoolYear(date).ShouldBe((date.Year - 1).ToString());
    }
}
