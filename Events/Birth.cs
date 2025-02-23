using Dates;
using Extensions;

namespace Events
{
	public class Birth : PersonalEvent
	{
		public Birth(string defaultName, StaticDate date, IDictionary<CalendarVersionName, string>? versionNames = null, bool ignoreAge = false) : base(EventType.Birth, defaultName, date, versionNames, ignoreAge)
		{
		}
		public Birth(string defaultName, string calculationDescription, IDictionary<CalendarVersionName, string>? versionNames = null, bool ignoreAge = false) : base(EventType.Birth, defaultName, calculationDescription, versionNames, ignoreAge)
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

			desc += "birthday";

			return desc;
		}
	}
}