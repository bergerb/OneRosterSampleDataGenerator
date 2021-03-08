using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

            string result = "";
            if (givenDateTime.Month > 6)
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
            MemoryStream stream = new MemoryStream(byteArray);
            return stream;
        }

    }
}
