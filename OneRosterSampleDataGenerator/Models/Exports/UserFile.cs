using CsvHelper.Configuration.Attributes;
using OneRosterSampleDataGenerator.Models.Interfaces;
using System;

namespace OneRosterSampleDataGenerator.Models.Exports;


public class UserFile : IExportable<User, UserFile>
{
    [Name("sourcedId")]
    public string SourcedId { get; set; } = null!;
    [Name("status")]
    public string Status { get; set; } = null!;
    [Name("dateLastModified")]
    [Format("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTime DateLastModified { get; set; }
    [Name("enabledUser")]
    public string EnabledUser { get; set; } = null!;
    [Name("orgSourcedIds")]
    public string OrgSourcedIds { get; set; } = null!;
    [Name("role")]
    public string Role { get; set; } = null!;
    [Name("username")]
    public string Username { get; set; } = null!;
    [Name("userIds")]
    public string UserIds { get; set; } = null!;
    [Name("givenName")]
    public string GivenName { get; set; } = null!;
    [Name("agentSourcedIds")]
    public string AgentSourcedIds { get; set; } = null!;
    [Name("familyName")]
    public string FamilyName { get; set; } = null!;
    [Name("middleName")]
    public string MiddleName { get; set; } = null!;
    [Name("identifier")]
    public string Identifier { get; set; } = null!;
    [Name("email")]
    public string Email { get; set; } = null!;
    [Name("sms")]
    public string Sms { get; set; } = null!;
    [Name("phone")]
    public string Phone { get; set; } = null!;
    [Name("grades")]
    public string Grades { get; set; } = null!;
    [Name("password")]
    public string Password { get; set; } = null!;

    public UserFile Map(User item)
    {
        return new()
        {
            DateLastModified = item.DateLastModified,
            Email = item.Email,
            EnabledUser = item.EnabledUser.ToString(),
            FamilyName = item.FamilyName,
            GivenName = item.GivenName,
            Identifier = item.Identifier,
            SourcedId = item.SourcedId.ToString(),
            Status = item.Status.ToString(),
            Username = item.UserName,
        };
    }
}
