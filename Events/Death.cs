using Dates;

namespace Events
{
	public class Death : PersonalEvent
	{
		public Death(string defaultName, StaticDate date, IDictionary<CalendarVersionName, string>? versionNames = null, bool ignoreAge = false) : base(EventType.Death, defaultName, date, versionNames, ignoreAge)
		{
		}
		public Death(string defaultName, string calculationDescription, IDictionary<CalendarVersionName, string>? versionNames = null, bool ignoreAge = false) : base(EventType.Death, defaultName, calculationDescription, versionNames, ignoreAge)
		{
		}

		public override string Describe(int year, CalendarVersionName version)
		{
			var desc = "Remembering " + VersionIdentifiers.GetValueOrDefault(version, DefaultIdentifier);

			if (!IgnoreAge)
			{
				var age = Date.AgeIn(year);
				if (age.HasValue)
				{
					desc += " (" + age.Value + " year" + (age == 1 ? string.Empty : "s") + ")";
				}
			}

			return desc;
		}
	}
}