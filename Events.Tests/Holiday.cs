using Dates;

namespace Events.Test
{
	public class Holiday
	{
		[Theory]
		[InlineData("Thanksgiving Day", "11-N4-5", 2024, "Thanksgiving Day", "2024-11-28")]
		[InlineData("Black Friday", "11-N4-5_+1", 2024, "Black Friday", "2024-11-29")]
		[InlineData("Pokémon Day", "02-27", 2024, "Pokémon Day", "2024-02-27")]
		[InlineData("Pokémon Day", "1996-02-27", 2024, "28th annual Pokémon Day", "2024-02-27")]
		public void Instantiation(string name, string dateStr, int inYear, string expectedDesc, string expectedDate)
		{
			CustomDate date = (dateStr.Contains("N") ? FloatingDate.Parse(dateStr) : StaticDate.Parse(dateStr));
			var holiday = new Events.Holiday(name, date);

			Assert.Equal(expectedDate, holiday.Date.CalculateDate(inYear).Value.ToString("yyyy-MM-dd"));
			Assert.Equal(expectedDesc, holiday.Describe(inYear));
		}

		[Fact]
		public void Instantiation_RequiresNonWhiteSpaceName()
		{
			var date = new Dates.StaticDate(1996, 10, 3);

			Assert.Throws<ArgumentNullException>(() =>
			{
				var holiday = new Events.Holiday(null, date);
			});
			Assert.Throws<ArgumentException>(() =>
			{
				var holiday = new Events.Holiday(string.Empty, date);
			});
			Assert.Throws<ArgumentException>(() =>
			{
				var holiday = new Events.Holiday("   ", date);
			});
		}

		[Fact]
		public void Instantiation_AllowsNullDate()
		{
			var holiday = new Events.Holiday("Tyler", null);
		}

		[Fact]
		public void DateCanBeSetAfterInstantiation()
		{
			var holiday = new Events.Holiday("Easter", null);
			Assert.Null(holiday.Date?.CalculateDate(2024));

			holiday.Date = new StaticDate(null, 3, 31);
			Assert.Equal(new DateOnly(2024, 3, 31), holiday.Date.CalculateDate(2024));
		}
	}
}