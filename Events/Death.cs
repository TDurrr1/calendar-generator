using Dates;
using System.Diagnostics.CodeAnalysis;

namespace Events
{
	public class Death : PersonalEvent
	{
		public Death([NotNull] string name, [NotNull] StaticDate date) : base(name, date, EventType.Death, null)
		{
			ArgumentNullException.ThrowIfNull(date);
		}

		public override string Describe(int year)
		{
			var desc = Identifier;
			var age = Date?.AgeIn(year);

			if (age.HasValue && age.Value != 0)
			{
				desc += " (" + age.Value + " year" + (age == 1 ? string.Empty : "s") + ")";
			}

			return desc;
		}
	}
}