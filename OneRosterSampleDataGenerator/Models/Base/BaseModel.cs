using System;

namespace OneRosterSampleDataGenerator.Models;

public abstract class BaseModel
{
    public DateTime CreatedAt => DateTime.Now;
    public StatusType Status { get; set; }
    public string Id { get; set; } = null!;
}
