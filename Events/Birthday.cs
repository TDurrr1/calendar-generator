using Dates;
using Extensions;

namespace Events
{
	public class Birthday : CalendarEvent
	{
		public string Possessive { get; init; }

		public Birthday(string name, CustomDate date, string possessive = null) : base(name, date, EventType.Birthday)
		{
			if (possessive == null)
			{
				Possessive = Identifier + "’s";
			}
			else
			{
				ArgumentException.ThrowIfNullOrWhiteSpace(possessive);
				Possessive = possessive.Trim();
			}
		}

		public string Describe(int year)
		{
			var desc = Possessive;

			var age = Date.AgeIn(year);
			if (age.HasValue)
			{
				desc += " " + age.Value.AsOrdinal();
			}

			return desc;
		}
	}
}
