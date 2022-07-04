using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace OneRosterSampleDataGenerator
{
    public class Utility
    {
        /// <summary>
        /// Returns the current string year of given date (if given)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetCurrentSchoolYear(DateTime? dateTime = null)
        {
            DateTime givenDateTime = dateTime ?? DateTime.Now;

            string result;
            if (givenDateTime.Month > 8 ||
                    (givenDateTime.Month == 8 && givenDateTime.Day > 15))
                result = givenDateTime.Year.ToString();
            else
                result = (givenDateTime.Year - 1).ToString();
            return result;
        }

        /// <summary>
        /// Returns the next school year of given date
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetNextSchoolYear(DateTime? dateTime = null)
        {
            DateTime givenDateTime = dateTime ?? DateTime.Now;
            return (int.Parse(GetCurrentSchoolYear(givenDateTime)) + 1).ToString();
        }

        /// <summary>
        /// String to Memory Stream
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static MemoryStream StringToMemoryStream(string contents)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(contents);
            MemoryStream stream = new(byteArray);
            return stream;
        }

        /// <summary>
        /// Create unique username for a teacher
        /// </summary>
        /// <param name="teachers"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public static string CreateTeacherUserName(List<Models.Staff> teachers, string firstName, string lastName)
        {
            var userName = string.Concat(firstName.AsSpan(0, 1), lastName);

            var userNameCount = 0;
            // If user name exists create a new one by adding 1
            while (teachers.Any(x => x.UserName == userName))
            {
                userNameCount++;
                userName = $"{userName}{userNameCount}";
            }

            return userName;
        }

    }
}
