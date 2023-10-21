namespace OneRosterSampleDataGenerator.Models.Interfaces
{
    public interface IExportable<T1, T2>
        where T1 : class
        where T2 : class
    {
        public T2 Map(T1 item);
    }
}
