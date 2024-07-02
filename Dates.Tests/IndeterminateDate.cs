using Dates;
using System.Globalization;

namespace CustomDate
{
	public class IndeterminateDate
	{
		[Fact]
		public void HappyPath()
		{
			var date = new Dates.IndeterminateDate("Note here");
			Assert.Throws<NotImplementedException>(() => date.CalculateDate(2024));
		}

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		public void SadPaths_ArgumentExceptions(string note)
		{
			Assert.Throws<ArgumentException>(() => new Dates.IndeterminateDate(note));
		}

		[Fact]
		public void SadPaths_ArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new Dates.IndeterminateDate(null));
		}

		[Fact]
		public void Age_IsNull()
		{
			var date = new Dates.IndeterminateDate("test");
			Assert.Null(date.AgeIn(2024));
		}

		[Fact]
		public void Parse()
		{
			var date = Dates.IndeterminateDate.Parse("!note", CultureInfo.CurrentCulture);
			Assert.NotNull(date);
		}

		[Fact]
		public void Parse_ArgumentNullExceptions()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				var date = Dates.IndeterminateDate.Parse(null);
			});
		}

		[Theory]
		[InlineData("!")]
		[InlineData("Doesn't start with !")]
		public void Parse_FormatExceptions(string input)
		{
			Assert.Throws<FormatException>(() =>
			{
				var date = Dates.IndeterminateDate.Parse(input);
			});
		}

		[Theory]
		[InlineData(null)]
		[InlineData("!test")]
		public void TryParse(string input)
		{
			Dates.IndeterminateDate date;
			if (Dates.IndeterminateDate.TryParse(input, null, out date))
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