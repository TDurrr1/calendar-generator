using Dates;
using Extensions;
using System.Collections.Frozen;

namespace Events
{
	public abstract class PersonalEvent : CalendarEvent
	{
		public static readonly FrozenSet<EventType> AllowedEventTypes = new HashSet<EventType>
		{
			EventType.Birth,
			EventType.Marriage,
			EventType.Death
		}.ToFrozenSet();

		public override EventType Type
		{
			get => base.Type;
			protected init
			{
				if (!AllowedEventTypes.Contains(value))
				{
					throw new ArgumentException("Must provide " + AllowedEventTypes.Select(t => t.ToString()).TextualJoin("or") + " as the event type", nameof(value));
				}

				base.Type = value;
			}
		}

		public PersonalEvent(EventType eventType, string defaultIdentifier, StaticDate date, IDictionary<CalendarVersionName, string>? versionIdentifiers = null, bool ignoreAge = false) : base(eventType, defaultIdentifier, date, versionIdentifiers, ignoreAge)
		{
		}

		public PersonalEvent(EventType eventType, string defaultIdentifier, string calculationDescription, IDictionary<CalendarVersionName, string>? versionIdentifiers = null, bool ignoreAge = false) : base(eventType, defaultIdentifier, calculationDescription, versionIdentifiers, ignoreAge)
		{
		}
	}
}
