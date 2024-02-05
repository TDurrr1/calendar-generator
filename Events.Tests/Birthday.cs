namespace Events.Test
{
	public class Birthday
	{
		[Theory]
		[InlineData("Tyler", 1996, 10, 3)]
		[InlineData("Monica", null, 10, 19)]
		public void Instantiation(string name, int? year, int month, int day)
		{
			var date = new Dates.StaticDate(year, month, day);
			var birthday = new Events.Birthday(name, date);

			Assert.Equal(name, birthday.Identifier);
			Assert.Equal(new DateOnly(2024, month, day), birthday.Date.CalculateDate(2024));
			if (year != null)
			{
				Assert.Equal(2024 - year, birthday.Date.AgeIn(2024));
			}
			else
			{
				Assert.Null(birthday.Date.AgeIn(2024));
			}
			Assert.Equal(EventType.Birthday, birthday.Type);
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