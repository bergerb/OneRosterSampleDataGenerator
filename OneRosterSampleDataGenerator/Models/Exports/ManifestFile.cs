using CsvHelper.Configuration.Attributes;
using OneRosterSampleDataGenerator.Models.Interfaces;

namespace OneRosterSampleDataGenerator.Models.Exports;

public class ManifestFile : IExportable<Manifest, ManifestFile>
{
    [Name("propertyName")]
    public string PropertyName { get; set; } = null!;
    [Name("value")]
    public string Value { get; set; } = null!;

    public ManifestFile Map(Manifest item)
    {
        return new()
        {
            PropertyName = item.PropertyName,
            Value = item.Value,
        };
    }
}
