using System.Collections.Generic;

namespace OneRosterSampleDataGenerator.Models.Interfaces
{
    public interface IGenerates<T>
        where T : class, new()
    {
        public List<T> Items { get; set; }
        public List<T> Generate();
    }
}
