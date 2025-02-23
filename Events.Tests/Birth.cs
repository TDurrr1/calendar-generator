using Dates;

namespace Events.Test
{
	public class Birth
	{
		[Theory]
		[InlineData("Tyler", 1996, 10, 3, 2024, "Tyler’s 28th birthday")]
		[InlineData("Baby", 2025, 10, 3, 2024, "Baby’s -1st birthday")]
		[InlineData("Tyler", 2024, 10, 3, 2024, "Tyler’s 0th birthday")]
		[InlineData("Monica", null, 10, 19, 2024, "Monica’s birthday")]
		public void Instantiation(string name, int? year, int month, int day, int inYear, string expectedDesc)
		{
			var date = new Dates.StaticDate(year, month, day);
			var birth = new Events.Birth(name, date);

			Assert.Equal(name, birth.DefaultIdentifier);
			Assert.Equal(new DateOnly(inYear, month, day), birth.Date.CalculateDate(inYear));
			if (year.HasValue)
			{
				Assert.Equal(inYear - year.Value, birth.Date.AgeIn(inYear));
			}
			else
			{
				Assert.Null(birth.Date.AgeIn(inYear));
			}
			Assert.Equal(EventType.Birth, birth.Type);

			Assert.Equal(expectedDesc, birth.Describe(inYear, CalendarVersionName.Durr));
		}

		[Fact]
		public void Instantiation_RequiresNonWhiteSpaceName()
		{
			var date = new Dates.StaticDate(1996, 10, 3);

			Assert.Throws<ArgumentNullException>(() =>
			{
				var birth = new Events.Birth(null, date);
			});
			Assert.Throws<ArgumentException>(() =>
			{
				var birth = new Events.Birth(string.Empty, date);
			});
			Assert.Throws<ArgumentException>(() =>
			{
				var birth = new Events.Birth("   ", date);
			});
		}

		[Fact]
		public void Instantiation_RequiresNonNullDate()
		{
			var name = "Tyler";
			StaticDate date = null;

			Assert.Throws<ArgumentNullException>(() =>
			{
				var birth = new Events.Birth(name, date);
			});
		}
	}
}