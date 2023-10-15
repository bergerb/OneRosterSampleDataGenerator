namespace OneRosterSampleDataGenerator.Models;

public abstract class BaseModel
{
    public StatusType Status { get; set; }
    public string Id { get; set; } = null!;
}
