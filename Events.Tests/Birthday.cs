namespace Events.Test
{
	public class Birthday
	{
		[Theory]
		[InlineData("Tyler", 1996, 10, 3, 2024, "Tyler’s 28th")]
		[InlineData("Baby", 2025, 10, 3, 2024, "Baby’s -1st")]
		[InlineData("Baby", 2024, 10, 3, 2024, "Baby’s 0th")]
		[InlineData("Monica", null, 10, 19, 2024, "Monica’s")]
		public void Instantiation(string name, int? year, int month, int day, int inYear, string expectedDesc)
		{
			var date = new Dates.StaticDate(year, month, day);
			var birthday = new Events.Birthday(name, date);

			Assert.Equal(name, birthday.Identifier);
			Assert.Equal(new DateOnly(inYear, month, day), birthday.Date.CalculateDate(inYear));
			if (year.HasValue)
			{
				Assert.Equal(inYear - year.Value, birthday.Date.AgeIn(inYear));
			}
			else
			{
				Assert.Null(birthday.Date.AgeIn(inYear));
			}
			Assert.Equal(EventType.Birthday, birthday.Type);

			Assert.Equal(expectedDesc, birthday.Describe(inYear));
		}

		[Fact]
		public void Instantiation_RequiresNonWhiteSpaceName()
		{
			var date = new Dates.StaticDate(1996, 10, 3);

			Assert.Throws<ArgumentNullException>(() =>
			{
				var birthday = new Events.Birthday(null, date);
			});
			Assert.Throws<ArgumentException>(() =>
			{
				var birthday = new Events.Birthday(string.Empty, date);
			});
			Assert.Throws<ArgumentException>(() =>
			{
				var birthday = new Events.Birthday("   ", date);
			});
		}

		[Fact]
		public void Instantiation_RequiresNonNullDate()
		{
			var name = "Tyler";

			Assert.Throws<ArgumentNullException>(() =>
			{
				var birthday = new Events.Birthday(name, null);
			});
		}
	}
}