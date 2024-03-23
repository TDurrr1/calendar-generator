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
		public string Identifier { get; protected init; }
		public CustomDate Date { get; protected init; }
		public EventType Type { get; protected init; }

		public CalendarEvent(string identifier, CustomDate date, EventType type)
		{
			ArgumentNullException.ThrowIfNull(identifier);
			ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
			Identifier = identifier.Trim();

			Date = date;

			Type = type;
		}
		public abstract string Describe(int year);
	}
}
