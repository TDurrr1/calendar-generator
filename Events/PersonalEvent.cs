using Dates;
using Extensions;

namespace Events
{
	public class PersonalEvent : CalendarEvent
	{
		public static readonly IReadOnlySet<EventType> AllowedEventTypes = new HashSet<EventType>
		{
			EventType.Birthday,
			EventType.Marriage,
			EventType.Death
		};

		public string Possessive { get; protected init; }

		public PersonalEvent(string name, StaticDate date, EventType eventType, string possessive = null) : base(name, date, eventType)
		{
			if (!AllowedEventTypes.Contains(eventType))
			{
				throw new ArgumentException(nameof(eventType));
			}

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

		public override string Describe(int year)
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
