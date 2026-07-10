using CsvHelper.Configuration.Attributes;
using OneRosterSampleDataGenerator.Models.Interfaces;
using System;

namespace OneRosterSampleDataGenerator.Models.Exports;

public class DemographicFile : IExportable<Demographic, DemographicFile>
{
    public static string FileName => "demographics.csv";
    [Name("sourcedId")]
    public string SourcedId { get; set; } = null!;
    [Name("status")]
    public string Status { get; set; } = null!;
    [Name("dateLastModified")]
    [Format("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTime DateLastModified { get; set; }
    [Name("birthDate")]
    [Format("yyyy-MM-dd")]
    public DateTime BirthDate { get; set; }
    [Name("sex")]
    public string Sex { get; set; } = null!;
    [Name("americanIndianOrAlaskaNative")]
    public string AmericanIndianOrAlaskaNative { get; set; } = null!;
    [Name("asian")]
    public string Asian { get; set; } = null!;
    [Name("blackOrAfricanAmerican")]
    public string BlackOrAfricanAmerican { get; set; } = null!;
    [Name("nativeHawaiianOrOtherPacificIslander")]
    public string NativeHawaiianOrOtherPacificIslander { get; set; } = null!;
    [Name("white")]
    public string White { get; set; } = null!;
    [Name("demographicRaceTwoOrMoreRaces")]
    public string DemographicRaceTwoOrMoreRaces { get; set; } = null!;
    [Name("hispanicOrLatinoEthnicity")]
    public string HispanicOrLatinoEthnicity { get; set; } = null!;
    [Name("countryOfBirthCode")]
    public string CountryOfBirthCode { get; set; } = null!;
    [Name("stateofBirthAbbreviation")]
    public string StateofBirthAbbreviation { get; set; } = null!;
    [Name("cityOfBirth")]
    public string CityOfBirth { get; set; } = null!;
    [Name("publicSchoolResidenceStatus")]
    public string PublicSchoolResidenceStatus { get; set; } = null!;

    public DemographicFile Map(Demographic item)
    {
        return new()
        {
            AmericanIndianOrAlaskaNative = item.AmericanIndianOrAlaskaNative.ToString().ToLower(),
            Asian = item.Asian.ToString().ToLower(),
            BirthDate = item.BirthDate,
            BlackOrAfricanAmerican = item.BlackOrAfricanAmerican.ToString().ToLower(),
            CityOfBirth = item.CityOfBirth,
            CountryOfBirthCode = item.CountryOfBirthCode,
            DateLastModified = item.DateLastModified,
            DemographicRaceTwoOrMoreRaces = item.DemographicRaceTwoOrMoreRaces.ToString().ToLower(),
            HispanicOrLatinoEthnicity = item.HispanicOrLatinoEthnicity.ToString().ToLower(),
            NativeHawaiianOrOtherPacificIslander = item.NativeHawaiianOrOtherPacificIslander.ToString().ToLower(),
            PublicSchoolResidenceStatus = item.PublicSchoolResidenceStatus,
            Sex = item.Sex,
            SourcedId = item.SourcedId.ToString(),
            StateofBirthAbbreviation = item.StateOfBirthAbbreviation,
            Status = item.Status.ToString(),
            White = item.White.ToString().ToLower(),
        };
    }
}
