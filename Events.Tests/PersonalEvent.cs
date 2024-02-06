namespace Events.Test
{
	public class PersonalEvent
	{
		[Theory]
		[InlineData("Tyler", 1996, 10, 3, "My", 2024, "My 28th")]
		[InlineData("Baby", 2025, 10, 3, null, 2024, "Baby’s -1st")]
		[InlineData("Baby", 2024, 10, 3, null, 2024, "Baby’s 0th")]
		[InlineData("Monica", null, 10, 19, null, 2024, "Monica’s")]
		public void Instantiation(string name, int? year, int month, int day, string possessive, int inYear, string expectedDesc)
		{
			var date = new Dates.StaticDate(year, month, day);
			var personalEvent = new Events.PersonalEvent(name, date, EventType.Birthday, possessive);

			Assert.Equal(name, personalEvent.Identifier);
			Assert.Equal(new DateOnly(inYear, month, day), personalEvent.Date.CalculateDate(inYear));
			if (year.HasValue)
			{
				Assert.Equal(inYear - year.Value, personalEvent.Date.AgeIn(inYear));
			}
			else
			{
				Assert.Null(personalEvent.Date.AgeIn(inYear));
			}
			Assert.Equal(EventType.Birthday, personalEvent.Type);

			Assert.Equal(expectedDesc, personalEvent.Describe(inYear));
		}

		[Fact]
		public void Instantiation_RequiresNonWhiteSpaceName()
		{
			var date = new Dates.StaticDate(1996, 10, 3);

			Assert.Throws<ArgumentNullException>(() =>
			{
				var birthday = new Events.PersonalEvent(null, date, EventType.Birthday);
			});
			Assert.Throws<ArgumentException>(() =>
			{
				var birthday = new Events.PersonalEvent(string.Empty, date, EventType.Birthday);
			});
			Assert.Throws<ArgumentException>(() =>
			{
				var birthday = new Events.PersonalEvent("   ", date, EventType.Birthday);
			});
		}

		[Fact]
		public void Instantiation_RequiresNonNullDate()
		{
			var name = "Tyler";

			Assert.Throws<ArgumentNullException>(() =>
			{
				var birthday = new Events.PersonalEvent(name, null, EventType.Birthday);
			});
		}

		[Theory]
		[InlineData(EventType.Death)]
		[InlineData(EventType.Holiday)]
		public void Instantiation_AllowedEventTypes(EventType eventType)
		{
			var name = "Tyler";
			var date = new Dates.StaticDate(1996, 10, 3);

			Assert.Throws<ArgumentException>(() =>
			{
				var birthday = new Events.PersonalEvent(name, date, eventType);
			});
		}
	}
}