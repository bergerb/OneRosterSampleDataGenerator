using OneRosterSampleDataGenerator.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace OneRosterSampleDataGenerator.Models.Base;

public abstract class Generator<T> : IGenerates<T>
    where T : class, new()
{
    public Generator(DateTime createdAt)
    {
        CreatedAt = createdAt;
    }

    public DateTime CreatedAt { get; set; }

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
