using Dates;

namespace Events
{
	public enum EventType
	{
		Birthday,
		Death,
		Marriage,
		Holiday
	}

	public abstract class CalendarEvent
	{
		public string Identifier { get; init; }
		public CustomDate Date { get; init; }
		public EventType Type { get; init; }

		public CalendarEvent(string identifier, CustomDate date, EventType type)
		{
			ArgumentNullException.ThrowIfNull(identifier);
			ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
			Identifier = identifier.Trim();

			ArgumentNullException.ThrowIfNull(date);
			Date = date;

			Type = type;
		}
	}
}
