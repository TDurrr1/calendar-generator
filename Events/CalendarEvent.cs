using Dates;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace Events
{
	public enum EventType
	{
		Birth,
		Death,
		Marriage,
		Observance
	}

	public abstract class CalendarEvent
	{
		public string Identifier { get; protected init; }
		public string Possessive { get; protected init; }
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
