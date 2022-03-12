using System;
using System.Collections.Generic;
using System.Text;

namespace OneRosterSampleDataGenerator.Models
{
    public abstract class BaseModel
    {

        public string Id { get; set; } = null!;

        public StatusType Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
