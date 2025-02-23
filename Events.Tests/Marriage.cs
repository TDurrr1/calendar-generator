using Dates;

namespace Events.Test
{
	public class Marriage
	{
		[Theory]
		[InlineData("Tyler and Monica", 1996, 10, 3, 2024, "Tyler and Monica’s 28th anniversary")]
		[InlineData("A and B", 2025, 10, 3, 2024, "A and B’s -1st anniversary")]
		[InlineData("Monica and Tyler", 2024, 10, 3, 2024, "Monica and Tyler’s 0th anniversary")]
		[InlineData("John", null, 10, 19, 2024, "John’s anniversary")]
		public void Instantiation(string name, int? year, int month, int day, int inYear, string expectedDesc)
		{
			var date = new Dates.StaticDate(year, month, day);
			var marriage = new Events.Marriage(name, date);

			Assert.Equal(name, marriage.DefaultIdentifier);
			Assert.Equal(new DateOnly(inYear, month, day), marriage.Date.CalculateDate(inYear));
			if (year.HasValue)
			{
				Assert.Equal(inYear - year.Value, marriage.Date.AgeIn(inYear));
			}
			else
			{
				Assert.Null(marriage.Date.AgeIn(inYear));
			}
			Assert.Equal(EventType.Marriage, marriage.Type);

			Assert.Equal(expectedDesc, marriage.Describe(inYear, CalendarVersionName.Durr));
		}

		[Fact]
		public void Instantiation_RequiresNonWhiteSpaceName()
		{
			var date = new Dates.StaticDate(1996, 10, 3);

			Assert.Throws<ArgumentNullException>(() =>
			{
				var marriage = new Events.Marriage(null, date);
			});
			Assert.Throws<ArgumentException>(() =>
			{
				var marriage = new Events.Marriage(string.Empty, date);
			});
			Assert.Throws<ArgumentException>(() =>
			{
				var marriage = new Events.Marriage("   ", date);
			});
		}

		[Fact]
		public void Instantiation_RequiresNonNullDate()
		{
			var name = "Tyler";
			StaticDate date = null;

			Assert.Throws<ArgumentNullException>(() =>
			{
				var marriage = new Events.Marriage(name, date);
			});
		}
	}
}