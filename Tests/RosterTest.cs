using NUnit.Framework;
using OneRosterSampleDataGenerator;

namespace Tests
{
    [TestFixture]
    public class RosterTest
    {
        public OneRoster OneRoster;

        [SetUp]
        public void TestStaffInitialize()
        {
            OneRoster = new OneRoster();
        }
    }
}