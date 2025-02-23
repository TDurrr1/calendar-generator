using Dates;

namespace Events.Test
{
	public class Death
	{
		[Theory]
		[InlineData("Tyler", 1996, 10, 3, 2024, "Remembering Tyler (28 years)")]
		[InlineData("Tyler", 2023, 10, 3, 2024, "Remembering Tyler (1 year)")]
		[InlineData("Baby", 2025, 10, 3, 2024, "Remembering Baby (-1 years)")]
		[InlineData("Baby", 2024, 10, 3, 2024, "Remembering Baby (0 years)")]
		[InlineData("Monica", null, 10, 19, 2024, "Remembering Monica")]
		public void Instantiation(string name, int? year, int month, int day, int inYear, string expectedDesc)
		{
			var date = new Dates.StaticDate(year, month, day);
			var death = new Events.Death(name, date);

			Assert.Equal(name, death.DefaultIdentifier);
			Assert.Equal(new DateOnly(inYear, month, day), death.Date.CalculateDate(inYear));
			if (year.HasValue)
			{
				Assert.Equal(inYear - year.Value, death.Date.AgeIn(inYear));
			}
			else
			{
				Assert.Null(death.Date.AgeIn(inYear));
			}
			Assert.Equal(EventType.Death, death.Type);

			Assert.Equal(expectedDesc, death.Describe(inYear, CalendarVersionName.Durr));
		}

		[Fact]
		public void Instantiation_RequiresNonWhiteSpaceName()
		{
			var date = new Dates.StaticDate(1996, 10, 3);

			Assert.Throws<ArgumentNullException>(() =>
			{
				var death = new Events.Death(null, date);
			});
			Assert.Throws<ArgumentException>(() =>
			{
				var death = new Events.Death(string.Empty, date);
			});
			Assert.Throws<ArgumentException>(() =>
			{
				var death = new Events.Death("   ", date);
			});
		}

		[Fact]
		public void Instantiation_RequiresNonNullDate()
		{
			var name = "Tyler";

			Assert.Throws<ArgumentNullException>(() =>
			{
				StaticDate date = null;
				var death = new Events.Death(name, date);
			});
		}
	}
}