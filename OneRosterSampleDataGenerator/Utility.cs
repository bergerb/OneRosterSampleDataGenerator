using System;
using System.Collections.Generic;
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
                result = (givenDateTime.Year -1).ToString();
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

        public class OneRosterEnums
        {
            public enum IMSClassType { homeroom, scheduled }
            public enum Gender { male, female }
            public enum Importance { primary, secondary }
            public enum OrgType { department, school, district, local, state, national }
            public enum RoleType { administrator, aide, guardian, parent, proctor, relative, student, teacher }
            public enum ScoreStatus { exempt, fully_graded, not_submitted, partially_graded, submitted }
            public enum SessionType { gradingPeriod, semester, schoolYear, term }
            public enum StatusType { active, tobedeleted, inactive }

        }

    }
}
