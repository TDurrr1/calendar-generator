using Dates;
using Extensions;

namespace Events
{
	public class Observance : CalendarEvent
	{
		public Observance(string defaultPossessive, CustomDate date, bool ignoreAge = false) : base(EventType.Observance, defaultPossessive, date, null, ignoreAge)
		{
		}

		public Observance(string defaultPossessive, string calculationDescription, bool ignoreAge = false) : base(EventType.Observance, defaultPossessive, calculationDescription, null, ignoreAge)
		{
		}

		public override string Describe(int inYear, CalendarVersionName version)
		{
			var desc = DefaultIdentifier;

			if (!IgnoreAge)
			{
				var age = Date.AgeIn(inYear);
				if (age.HasValue)
				{
					desc = (age.Value + 1).AsOrdinal() + " annual " + desc;
				}
			}

			return desc;
		}
	}
}
