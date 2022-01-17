using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Demographic : BaseModel
    {
        public Guid SourcedId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Sex { get; set; }
        public bool AmericanIndianOrAlaskaNative { get; set; }
        public bool BlackOrAfricanAmerican { get; set; }
        public bool NativeHawaiianOrOtherPacificIslander { get; set; }
        public bool White { get; set; }
        public bool DemographicRaceTwoOrMoreRaces { get; set; }
        public bool HispanicOrLationEthnicity { get; set; }
        public string CountryOfBirthCode { get; set; }
        public string StateOfBirthAbbreviation { get; set; }
        public string CityOfBirth { get; set; }
        public string PublicSchoolResidenceStatus { get; set; }
    }
}
