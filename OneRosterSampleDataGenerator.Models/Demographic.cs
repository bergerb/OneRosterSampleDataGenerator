using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Demographic : BaseModel
    {
        public Guid sourcedId { get; set; }
        public DateTime birthDate { get; set; }
        public string sex { get; set; }
        public bool americanIndianOrAlaskaNative { get; set; }
        public bool blackOrAfricanAmerican { get; set; }
        public bool nativeHawaiianOrOtherPacificIslander { get; set; }
        public bool white { get; set; }
        public bool demographicRaceTwoOrMoreRaces { get; set; }
        public bool hispanicOrLationEthnicity { get; set; }
        public string countryOfBirthCode { get; set; }
        public string stateOfBirthAbbreviation { get; set; }
        public string cityOfBirth { get; set; }
        public string publicSchoolResidenceStatus { get; set; }
    }
}
