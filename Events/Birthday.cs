using Dates;

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
	}
}
