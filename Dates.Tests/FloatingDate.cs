using System.Globalization;

namespace ICustomDate
{
	public class FloatingDate
	{
		[Theory]
		[InlineData(10, 1, 5, null, 1996, "1996-10-03")]
		[InlineData(10, 3, 5, null, 1995, "1995-10-19")]
		[InlineData(10, 1, 5, 1, 1996, "1996-10-04")]
		[InlineData(10, 3, 5, 1, 1995, "1995-10-20")]
		[InlineData(10, 1, 5, -1, 1996, "1996-10-02")]
		[InlineData(10, 3, 5, -1, 1995, "1995-10-18")]
		[InlineData(1, 1, 2, null, 2024, "2024-01-01")]
		[InlineData(1, 1, 2, -1, 2024, "2023-12-31")]
		[InlineData(12, 5, 1, 1, 2023, "2024-01-01")]
		[InlineData(2, 4, 4, 1, 2024, "2024-02-29")]
		[InlineData(3, 1, 7, -1, 2025, "2025-02-28")]
		[InlineData(2, 10, 1, -1, 2024, "2024-04-06")]
		[InlineData(2, 10, 1, null, 2024, "2024-04-07")]
		[InlineData(2, 10, 1, 1, 2024, "2024-04-08")]
		public void Instantiation(int month, int instance, int dayOfWeek, int? offset, int inYear, string expected)
		{
			var date = new Dates.FloatingDate(month, instance, dayOfWeek, offset);
			Assert.Equal(expected, date.CalculateDate(inYear).ToString("yyyy-MM-dd"));
		}

		[Theory]
		[InlineData(0, 1, 1, null, 2024)]
		[InlineData(13, 1, 1, null, 2024)]
		[InlineData(1, 0, 1, null, 2024)]
		[InlineData(1, 1, 0, null, 2024)]
		[InlineData(1, 1, 8, null, 2024)]
		[InlineData(1, 1, 1, null, 0)]
		public void Exceptions(int month, int instance, int dayOfWeek, int? offset, int inYear)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				var date = new Dates.FloatingDate(month, instance, dayOfWeek, offset);
				var str = date.CalculateDate(inYear).ToString("yyyy-MM-dd");
			});
		}

		[Fact]
		public void Age_IsNull()
		{
			var rand = new Random();
			for (var i = 1; i <= 100; i++)
			{
				var date = new Dates.FloatingDate(rand.Next(1, 12), rand.Next(1, 5), rand.Next(1, 7));
				Assert.Null(date.AgeIn(rand.Next(1900, 2100)));
			}
		}

		[Theory]
		[InlineData("02-N1-1", 2024, "2024-02-04", null)]
		[InlineData("02-N2-1", 2024, "2024-02-11", null)]
		[InlineData("02-N3-2", 2024, "2024-02-19", null)]
		[InlineData("02-N5-5", 2024, "2024-02-29", null)]
		[InlineData("02-N5-5", 2025, "2025-03-06", null)]
		public void Parse(string input, int inYear, string expectedStr, int? expectedAge)
		{
			var date = Dates.FloatingDate.Parse(input, CultureInfo.CurrentCulture);
			Assert.Equal(expectedStr, date.CalculateDate(inYear).ToString("yyyy-MM-dd"));
			Assert.Equal(expectedAge, date.AgeIn(inYear));
		}

		[Fact]
		public void Parse_ArgumentNullExceptions()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				var date = Dates.FloatingDate.Parse(null);
			});
		}

		[Theory]
		[InlineData("")]
		[InlineData("01-01")]
		[InlineData("2023-01-01")]
		[InlineData("01-N01-1")]
		[InlineData("01-N1-01")]
		[InlineData("01-N1")]
		[InlineData("01-N1-1_-1000")]
		[InlineData("01-N1-1_+1000")]
		[InlineData("01-N1-1_1000")]
		public void Parse_FormatExceptions(string input)
		{
			Assert.Throws<FormatException>(() =>
			{
				var date = Dates.FloatingDate.Parse(input);
			});
		}

		[Theory]
		[InlineData("00-N1-1")]
		[InlineData("13-N1-1")]
		[InlineData("01-N0-1")]
		[InlineData("01-N1-0")]
		[InlineData("01-N1-8")]
		public void Parse_OverflowExceptions(string input)
		{
			Assert.Throws<OverflowException>(() =>
			{
				var date = Dates.FloatingDate.Parse(input);
			});
		}

		[Theory]
		[InlineData(null)]
		[InlineData("01-N1-1_1")]
		public void TryParse(string input)
		{
			Dates.FloatingDate date;
			if (Dates.FloatingDate.TryParse(input, null, out date))
			{
				Assert.NotNull(date);
			}
			else
			{
				Assert.Null(date);
			}
		}
	}
}