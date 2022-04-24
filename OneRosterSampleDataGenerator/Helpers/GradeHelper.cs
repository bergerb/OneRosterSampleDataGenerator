namespace OneRosterSampleDataGenerator.Helpers
{
    public static class GradeHelper
    {
        // Elementary
        public const string Kintergarden = "KG";
        public const string First = "01";
        public const string Second = "02";
        public const string Third = "03";
        public const string Fourth = "04";
        public const string Fifth = "05";
        // Middle
        public const string Sixth = "06";
        public const string Seventh = "07";
        public const string Eighth = "08";
        // High
        public const string Ninth = "09";
        public const string Tenth = "10";
        public const string Eleventh = "11";
        public const string Twelveth = "12";

        public readonly static string[] SchoolLevels =
        {
            "Elementary",
            "Middle",
            "High"
        };

        public readonly static string[] Elementary =
        {
            Kintergarden,
            First,
            Second,
            Third,
            Fourth,
            Fifth,
        };

        public readonly static string[] Middle =
        {
            Sixth,
            Seventh,
            Eighth,
        };

        public readonly static string[] High =
        {
            Ninth,
            Tenth,
            Eleventh,
            Twelveth,
        };
    }
}
