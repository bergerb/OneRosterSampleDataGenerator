using CsvHelper.Configuration.Attributes;
using OneRosterSampleDataGenerator.Models.Interfaces;
using System;

namespace OneRosterSampleDataGenerator.Models.Exports;

public class DemographicFile : IExportable<Demographic, DemographicFile>
{
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
    [Name("nativeAmericanOrOtherPacificIslander")]
    public string NativeAmericanOrOtherPacificIslander { get; set; } = null!;
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
            BirthDate = item.BirthDate,
            DateLastModified = item.DateLastModified,
            Sex = item.Sex,
            SourcedId = item.SourcedId.ToString(),
            Status = item.Status.ToString(),
        };
    }
}
