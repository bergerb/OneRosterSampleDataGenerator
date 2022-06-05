using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Demographic : BaseModel
    {
        public bool AmericanIndianOrAlaskaNative { get; set; }
        public bool BlackOrAfricanAmerican { get; set; }
        public bool NativeHawaiianOrOtherPacificIslander { get; set; }
        public bool DemographicRaceTwoOrMoreRaces { get; set; }
        public bool HispanicOrLationEthnicity { get; set; }
        public bool White { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DateLastModified => CreatedAt;
        public Guid SourcedId { get; set; }
        public string CityOfBirth { get; set; } = null!;
        public string CountryOfBirthCode { get; set; } = null!;
        public string PublicSchoolResidenceStatus { get; set; } = null!;
        public string Sex { get; set; } = null!;
        public string StateOfBirthAbbreviation { get; set; } = null!;
    }
}
