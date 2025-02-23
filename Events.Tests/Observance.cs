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
			Assert.Equal(expectedDesc, observance.Describe(inYear, CalendarVersionName.Durr));
		}

		[Fact]
		public void Instantiation_RequiresNonWhiteSpaceName()
		{
			var date = new Dates.StaticDate(1996, 10, 3);

			Assert.Throws<ArgumentNullException>(() =>
			{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
				var observance = new Events.Observance(null, date);
#pragma warning restore CS8625
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
		public void Instantiation_DoesNotAllowNullDate()
		{
			Assert.Throws<ArgumentNullException>(static () =>
			{
#pragma warning disable CS8600, CS8604 // Converting null literal or possible null value to non-nullable type; possible null reference argument.
				StaticDate date = null;
				var observance = new Events.Observance("Tyler", date);
#pragma warning restore CS8600, CS8604
			});
		}

		[Fact]
		public void DateSetting()
		{
			var expectedDate = new DateOnly(2024, 3, 31);
			var calculationDescription = "Just Google it.";

			var observance = new Events.Observance("Easter", calculationDescription);
			var exception = Assert.Throws<DateUnspecifiedException>(() =>
			{
				var actualDate = observance.Date.CalculateDate(expectedDate.Year);
			});
			Assert.Equal(calculationDescription, exception.Message);

			observance.Date = new StaticDate(null, expectedDate.Month, expectedDate.Day);
			Assert.Equal(expectedDate, observance.Date.CalculateDate(expectedDate.Year));

			Assert.Throws<DateSpecifiedException>(() =>
			{
				observance.Date = new StaticDate(1996, 10, 03);
			});
			Assert.Equal(expectedDate, observance.Date.CalculateDate(expectedDate.Year));
		}
	}
}