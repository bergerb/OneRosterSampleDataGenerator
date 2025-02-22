using OneRosterSampleDataGenerator.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace OneRosterSampleDataGenerator.Models.Base;

public abstract class Generator<T>(DateTime createdAt) : IGenerates<T>
    where T : class, new()
{
    public DateTime CreatedAt { get; set; } = createdAt;

    public List<T> Items { get; set; } = [];

    public void AddItem(T item)
    {
        this.Items.Add(item);
    }

    public virtual List<T> Generate()
    {
        throw new NotImplementedException();
    }

    public int RunningId { get; set; } = 0;
}
