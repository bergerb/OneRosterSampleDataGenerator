using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public class Building
    {
        public Guid id { get; set; }
        public string number { get; set; }
        public string name { get; set; }
        public List<Grade> gradesOffer { get; set; }
    }
}
