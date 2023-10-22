using OneRosterSampleDataGenerator.Models;
using System;

namespace OneRosterSampleDataGenerator.Extensions
{
    internal static class UserExtensions
    {
        public static User DeactivateUser(this User user, DateTime dateLastModified)
        {
            user.Status = StatusType.tobedeleted;
            user.DateLastModified = dateLastModified;

            return user;
        }
    }
}
