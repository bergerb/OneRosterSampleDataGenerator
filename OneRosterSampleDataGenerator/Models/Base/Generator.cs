using OneRosterSampleDataGenerator.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace OneRosterSampleDataGenerator.Models.Base;

public abstract record Generator<T> : IGenerates<T>
    where T : class, new()
{
    public List<T> Items { get; set; } = new List<T>();

    public void AddItem(T item)
    {
        Items.Add(item);
    }

    public virtual List<T> Generate()
    {
        throw new NotImplementedException();
    }

    public int RunningId { get; set; } = 0;
}
