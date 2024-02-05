using Dates;
using System.Globalization;

namespace ICustomDate
{
	public class StaticDate
	{
		[Theory]
		[InlineData(2023, 1, 1, null, "2023-01-01")]
		[InlineData(2100, 1, 1, null, "2100-01-01")]
		[InlineData(2024, 2, 29, null, "2024-02-29")]
		[InlineData(2023, 1, 1, 1, "2023-01-02")]
		[InlineData(2023, 1, 1, 0, "2023-01-01")]
		[InlineData(2023, 1, 1, -1, "2022-12-31")]
		[InlineData(2100, 1, 1, -1, "2099-12-31")]
		[InlineData(2024, 2, 28, 1, "2024-02-29")]
		[InlineData(2024, 2, 28, 2, "2024-03-01")]
		[InlineData(2024, 3, 1, -1, "2024-02-29")]
		[InlineData(2100, 2, 28, 1, "2100-03-01")]
		[InlineData(2023, 1, 1, -400, "2021-11-27")]
		[InlineData(2023, 1, 1, 400, "2024-02-05")]
		public void WithYear(int year, int month, int day, int? offset, string expected)
		{
			var date = new Dates.StaticDate(year, month, day, offset);
			Assert.Equal(expected, date.CalculateDate(year).ToString("yyyy-MM-dd"));
		}

		[Theory]
		[InlineData(1, 1, 1, 2023, "2023-01-02")]
		[InlineData(1, 1, null, 2023, "2023-01-01")]
		[InlineData(1, 1, -1, 2023, "2022-12-31")]
		[InlineData(1, 1, -1, 2100, "2099-12-31")]
		[InlineData(2, 28, 1, 2024, "2024-02-29")]
		[InlineData(2, 28, 1, 2023, "2023-03-01")]
		[InlineData(2, 28, 2, 2024, "2024-03-01")]
		[InlineData(3, 1, -1, 2024, "2024-02-29")]
		[InlineData(3, 1, -1, 2023, "2023-02-28")]
		[InlineData(2, 28, 1, 2100, "2100-03-01")]
		[InlineData(1, 1, -400, 2023, "2021-11-27")]
		[InlineData(1, 1, 400, 2023, "2024-02-05")]
		public void WithoutYear(int month, int day, int? offset, int inYear, string expected)
		{
			var date = new Dates.StaticDate(null, month, day, offset);
			Assert.Equal(expected, date.CalculateDate(inYear).ToString("yyyy-MM-dd"));
		}

		[Theory]
		[InlineData(2024, 0, LeapDayAdjustment.March1, 2028, "2028-02-29")]
		[InlineData(2024, 0, LeapDayAdjustment.February28, 2028, "2028-02-29")]
		[InlineData(2024, 0, LeapDayAdjustment.ThrowException, 2028, "2028-02-29")]
		[InlineData(2024, 0, LeapDayAdjustment.March1, 2025, "2025-03-01")]
		[InlineData(2024, 0, LeapDayAdjustment.February28, 2025, "2025-02-28")]
		[InlineData(2024, 0, LeapDayAdjustment.ThrowException, 2025, null)]
		[InlineData(2024, 1, LeapDayAdjustment.March1, 2028, "2028-03-01")]
		[InlineData(2024, 1, LeapDayAdjustment.February28, 2028, "2028-03-01")]
		[InlineData(2024, 1, LeapDayAdjustment.ThrowException, 2028, "2028-03-01")]
		[InlineData(2024, 1, LeapDayAdjustment.March1, 2025, "2025-03-02")] // Day after March 1!
		[InlineData(2024, 1, LeapDayAdjustment.February28, 2025, "2025-03-01")]
		[InlineData(2024, 1, LeapDayAdjustment.ThrowException, 2025, "2025-03-01")]
		[InlineData(2024, -1, LeapDayAdjustment.March1, 2028, "2028-02-28")]
		[InlineData(2024, -1, LeapDayAdjustment.February28, 2028, "2028-02-28")]
		[InlineData(2024, -1, LeapDayAdjustment.ThrowException, 2028, "2028-02-28")]
		[InlineData(2024, -1, LeapDayAdjustment.March1, 2025, "2025-02-28")]
		[InlineData(2024, -1, LeapDayAdjustment.February28, 2025, "2025-02-27")] // Day before February 28!
		[InlineData(2024, -1, LeapDayAdjustment.ThrowException, 2025, "2025-02-28")]
		[InlineData(null, 0, LeapDayAdjustment.March1, 2028, "2028-02-29")]
		[InlineData(null, 0, LeapDayAdjustment.February28, 2028, "2028-02-29")]
		[InlineData(null, 0, LeapDayAdjustment.ThrowException, 2028, "2028-02-29")]
		[InlineData(null, 0, LeapDayAdjustment.March1, 2025, "2025-03-01")]
		[InlineData(null, 0, LeapDayAdjustment.February28, 2025, "2025-02-28")]
		[InlineData(null, 0, LeapDayAdjustment.ThrowException, 2025, null)]
		[InlineData(null, 1, LeapDayAdjustment.March1, 2028, "2028-03-01")]
		[InlineData(null, 1, LeapDayAdjustment.February28, 2028, "2028-03-01")]
		[InlineData(null, 1, LeapDayAdjustment.ThrowException, 2028, "2028-03-01")]
		[InlineData(null, 1, LeapDayAdjustment.March1, 2025, "2025-03-02")] // Day after March 1!
		[InlineData(null, 1, LeapDayAdjustment.February28, 2025, "2025-03-01")]
		[InlineData(null, 1, LeapDayAdjustment.ThrowException, 2025, "2025-03-01")]
		[InlineData(null, -1, LeapDayAdjustment.March1, 2028, "2028-02-28")]
		[InlineData(null, -1, LeapDayAdjustment.February28, 2028, "2028-02-28")]
		[InlineData(null, -1, LeapDayAdjustment.ThrowException, 2028, "2028-02-28")]
		[InlineData(null, -1, LeapDayAdjustment.March1, 2025, "2025-02-28")]
		[InlineData(null, -1, LeapDayAdjustment.February28, 2025, "2025-02-27")] // Day before February 28!
		[InlineData(null, -1, LeapDayAdjustment.ThrowException, 2025, "2025-02-28")]
		private void LeapDay(int? year, int? offset, LeapDayAdjustment lda, int inYear, string expected)
		{
			var date = new Dates.StaticDate(year, 2, 29, offset, lda);

			if (!DateTime.IsLeapYear(inYear) && lda == LeapDayAdjustment.ThrowException)
			{
				Assert.Throws<InvalidOperationException>(() => date.CalculateDate(inYear));
			}
			else
			{
				Assert.Equal(expected, date.CalculateDate(inYear).ToString("yyyy-MM-dd"));
			}
		}

		[Theory]
		[InlineData(-100, 1, 1, 2023)]
		[InlineData(2023, 0, 1, 2023)]
		[InlineData(2023, 13, -1, 2023)]
		[InlineData(2023, 1, 0, 2023)]
		[InlineData(2023, 1, 32, 2023)]
		[InlineData(2023, 2, 30, 2023)]
		[InlineData(2024, 2, 30, 2024)]
		[InlineData(2025, 2, 29, 2025)]
		[InlineData(2025, 2, 28, 0)]
		public void Exceptions(int? year, int month, int day, int inYear)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				var date = new Dates.StaticDate(year, month, day);
				var str = date.CalculateDate(inYear).ToString("yyyy-MM-dd");
			});
		}

		[Theory]
		[InlineData(2023, 1, 1, 2022, -1)]
		[InlineData(2023, 1, 1, 2023, 0)]
		[InlineData(2023, 1, 1, 2024, 1)]
		[InlineData(2023, 1, 1, 2123, 100)]
		[InlineData(null, 1, 1, 2023, null)]
		public void Age(int? year, int month, int day, int inYear, int? expected)
		{
			var date = new Dates.StaticDate(year, month, day);
			Assert.Equal(expected, date.AgeIn(inYear));
		}

		[Theory]
		[InlineData("2023-01-01", 2023, "2023-01-01", 0)]
		[InlineData("01-01", 2023, "2023-01-01", null)]
		[InlineData("2023-01-01_-1", 2023, "2022-12-31", 0)]
		[InlineData("2023-01-01_0", 2023, "2023-01-01", 0)]
		[InlineData("2023-01-01_1", 2023, "2023-01-02", 0)]
		[InlineData("2023-01-01_+1", 2023, "2023-01-02", 0)]
		[InlineData("2023-01-01_+50", 2023, "2023-02-20", 0)]
		[InlineData("01-01_-1", 2023, "2022-12-31", null)]
		[InlineData("01-01_0", 2023, "2023-01-01", null)]
		[InlineData("01-01_1", 2023, "2023-01-02", null)]
		[InlineData("01-01_+1", 2023, "2023-01-02", null)]
		[InlineData("01-01_+50", 2023, "2023-02-20", null)]
		[InlineData("2023-01-01", 2030, "2030-01-01", 7)]
		[InlineData("01-01", 2030, "2030-01-01", null)]
		[InlineData("2023-01-01_-1", 2030, "2029-12-31", 7)]
		[InlineData("2023-01-01_0", 2030, "2030-01-01", 7)]
		[InlineData("2023-01-01_1", 2030, "2030-01-02", 7)]
		[InlineData("2023-01-01_+1", 2030, "2030-01-02", 7)]
		[InlineData("2023-01-01_+50", 2030, "2030-02-20", 7)]
		[InlineData("01-01_-1", 2030, "2029-12-31", null)]
		[InlineData("01-01_0", 2030, "2030-01-01", null)]
		[InlineData("01-01_1", 2030, "2030-01-02", null)]
		[InlineData("01-01_+1", 2030, "2030-01-02", null)]
		[InlineData("01-01_+50", 2030, "2030-02-20", null)]
		public void Parse(string input, int inYear, string expectedStr, int? expectedAge)
		{
			var date = Dates.StaticDate.Parse(input, CultureInfo.CurrentCulture);
			Assert.Equal(expectedStr, date.CalculateDate(inYear).ToString("yyyy-MM-dd"));
			Assert.Equal(expectedAge, date.AgeIn(inYear));
		}

		[Fact]
		public void Parse_ArgumentNullExceptions()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				var date = Dates.StaticDate.Parse(null);
			});
		}

		[Theory]
		[InlineData("")]
		[InlineData("2023-01")]
		[InlineData("01-N1-1")]
		[InlineData("01-1")]
		[InlineData("-01-01")]
		[InlineData("01-01_")]
		[InlineData("01-01_+-1")]
		[InlineData("01-01_1000")]
		[InlineData("01-01+1000")]
		[InlineData("01-01-1000")]
		[InlineData("2023-01-1")]
		[InlineData("203-01-01")]
		[InlineData("20230101")]
		[InlineData("2023-01-01_")]
		[InlineData("2023-01-01_+-1")]
		[InlineData("2023-01-01_1000")]
		[InlineData("2023-01-01+1000")]
		[InlineData("2023-01-01-1000")]
		public void Parse_FormatExceptions(string input)
		{
			Assert.Throws<FormatException>(() =>
			{
				var date = Dates.StaticDate.Parse(input);
			});
		}

		[Theory]
		[InlineData("2023-00-01")]
		[InlineData("2023-13-01")]
		[InlineData("2023-01-00")]
		[InlineData("2023-01-32")]
		[InlineData("2023-02-29")]
		[InlineData("2024-02-30")]
		public void Parse_OverflowExceptions(string input)
		{
			Assert.Throws<OverflowException>(() =>
			{
				var date = Dates.StaticDate.Parse(input);
			});
		}

		[Theory]
		[InlineData(null)]
		[InlineData("2023-01-01")]
		public void TryParse(string input)
		{
			Dates.StaticDate date;
			if (Dates.StaticDate.TryParse(input, null, out date))
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