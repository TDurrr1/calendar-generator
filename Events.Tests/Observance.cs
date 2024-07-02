using Dates;

namespace Events.Test
{
	public class Observance
	{
		[Theory]
		[InlineData("Thanksgiving Day", "11-N4-5", 2024, "Thanksgiving Day", "2024-11-28")]
		[InlineData("Black Friday", "11-N4-5_+1", 2024, "Black Friday", "2024-11-29")]
		[InlineData("Pokémon Day", "02-27", 2024, "Pokémon Day", "2024-02-27")]
		[InlineData("Pokémon Day", "1996-02-27", 2024, "29th annual Pokémon Day", "2024-02-27")]
		[InlineData("Memorial Day", "1868-05-NL-2", 1868, "1st annual Memorial Day", "1868-05-25")]
		[InlineData("Memorial Day", "1868-05-NL-2", 2024, "157th annual Memorial Day", "2024-05-27")]
		public void Instantiation(string name, string dateStr, int inYear, string expectedDesc, string expectedDate)
		{
			CustomDate date = CustomDate.Parse(dateStr, null);
			var observance = new Events.Observance(name, date);

			Assert.Equal(expectedDate, observance.Date.CalculateDate(inYear).ToString("yyyy-MM-dd"));
			Assert.Equal(expectedDesc, observance.Describe(inYear));
		}

		[Fact]
		public void Instantiation_RequiresNonWhiteSpaceName()
		{
			var date = new Dates.StaticDate(1996, 10, 3);

			Assert.Throws<ArgumentNullException>(() =>
			{
				var observance = new Events.Observance(null, date);
			});
			Assert.Throws<ArgumentException>(() =>
			{
				var observance = new Events.Observance(string.Empty, date);
			});
			Assert.Throws<ArgumentException>(() =>
			{
				var observance = new Events.Observance("   ", date);
			});
		}

		[Fact]
		public void Instantiation_AllowsNullDate()
		{
			var observance = new Events.Observance("Tyler", null);
		}

		[Fact]
		public void DateCanBeSetAfterInstantiation()
		{
			var observance = new Events.Observance("Easter", null);
			Assert.Null(observance.Date?.CalculateDate(2024));

			observance.Date = new StaticDate(null, 3, 31);
			Assert.Equal(new DateOnly(2024, 3, 31), observance.Date.CalculateDate(2024));

			observance.Date = new IndeterminateDate("Can't programmatically calculate date");
			Assert.Throws<NotImplementedException>(() => observance.Date.CalculateDate(2024));
		}
	}
}