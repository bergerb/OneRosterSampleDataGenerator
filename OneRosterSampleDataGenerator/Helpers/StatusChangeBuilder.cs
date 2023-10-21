using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OneRosterSampleDataGenerator.Helpers;

/// <summary>
/// Track changes of a series of related status changes
/// Outputs to given filename
/// </summary>
public class StatusChangeBuilder
{
    private readonly string _fileName = null!;
    private readonly List<_event> _events = new();

    private record _event(EventAction eventAction, Type @type, string message)
    {
        public string createdAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
    };

    public enum EventAction
    {
        Created,
        Deactivated,
    }

    public enum @Type
    {
        File,
        Enrollment,
        Staff,
        Student,
    }

    public StatusChangeBuilder(string fileName)
    {
        _fileName = fileName;
    }

    /// <summary>
    /// Add Event to Status Log
    /// </summary>
    /// <param name="event"></param>
    /// <param name="type"></param>
    /// <param name="message"></param>
    public StatusChangeBuilder AddEvent(EventAction @event, @Type @type, string message)
    {
        _events.Add(new(@event, type, message));

        return this;
    }

    /// <summary>
    /// Writes to given output file
    /// </summary>
    public void OutputChangeLog()
    {
        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine("| Event Action | Type | CreatedAt | Message |");
        stringBuilder.AppendLine("| ---- |  ----  |  ----  |  ----  |");
        foreach (var @event in _events)
        {
            stringBuilder.AppendLine($"| {@event.eventAction} | {@event.type} | {@event.createdAt} | {@event.message} |");
        }

        if (File.Exists(_fileName))
        {
            File.Delete(_fileName);
        }

        File.WriteAllText(_fileName, stringBuilder.ToString());
    }
}
