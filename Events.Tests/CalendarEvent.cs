using Dates;
using System.Xml.Linq;

namespace Events.Test
{
	public class CalendarEvent
	{
		[Theory]
		[InlineData("Tyler Durr", "Tyler", "10-03", 2026, "Tyler Durr’s birthday", "Tyler’s birthday")]
		[InlineData("Tyler Durr", "Tyler", "1996-10-03", 2026, "Tyler Durr’s 30th birthday", "Tyler’s 30th birthday")]
		public void VersionIdentifiers(string name, string durrName, string dateStr, int inYear, string expectedDefaultDesc, string expectedDurrDesc)
		{
			StaticDate date = StaticDate.Parse(dateStr, null);
			var versionNames = new Dictionary<CalendarVersionName, string>()
			{
				{ CalendarVersionName.Durr, durrName }
			};
			var birth = new Events.Birth(name, date, versionNames);

			Assert.Equal(expectedDefaultDesc, birth.Describe(inYear, CalendarVersionName.Bergs));
			Assert.Equal(expectedDurrDesc, birth.Describe(inYear, CalendarVersionName.Durr));
		}

		[Fact]
		public void IgnoreAge()
		{
			StaticDate date = StaticDate.Parse("1996-10-03", null);
			var ageIgnored = new Events.Birth("Tyler", date, ignoreAge: false);
			var ageConsidered = new Events.Birth("Tyler", date, ignoreAge: true);

			Assert.Equal("Tyler’s 28th birthday", ageIgnored.Describe(2024, CalendarVersionName.Durr));
			Assert.Equal("Tyler’s birthday", ageConsidered.Describe(2024, CalendarVersionName.Durr));
		}
	}
}