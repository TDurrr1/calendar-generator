using Dates;
using Extensions;

namespace Events
{
	public class Marriage : PersonalEvent
	{
		public Marriage(string defaultName, StaticDate date, IDictionary<CalendarVersionName, string>? versionNames = null, bool ignoreAge = false) : base(EventType.Marriage, defaultName, date, versionNames, ignoreAge)
		{
		}
		public Marriage(string defaultName, string calculationDescription, IDictionary<CalendarVersionName, string>? versionNames = null, bool ignoreAge = false) : base(EventType.Marriage, defaultName, calculationDescription, versionNames, ignoreAge)
		{
		}

		public override string Describe(int year, CalendarVersionName version)
		{
			var desc = VersionIdentifiers.GetValueOrDefault(version, DefaultIdentifier) + "’s ";

			if (!IgnoreAge)
			{
				var age = Date.AgeIn(year);
				if (age.HasValue)
				{
					desc += age.Value.AsOrdinal() + " ";
				}
			}

			desc += "anniversary";

			return desc;
		}
	}
}