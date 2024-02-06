using Dates;

namespace Events
{
	public class Death : PersonalEvent
	{
		public Death(string name, StaticDate date) : base(name, date, EventType.Death, null)
		{
		}

		public override string Describe(int year)
		{
			var desc = Identifier;

			var age = Date.AgeIn(year);
			if (age.HasValue && age.Value != 0)
			{
				desc += " (" + age.Value + " year" + (age == 1 ? string.Empty : "s") + ")";
			}

			return desc;
		}
	}
}