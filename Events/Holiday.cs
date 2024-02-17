using Dates;
using Extensions;

namespace Events
{
	public class Holiday : CalendarEvent
	{
		private CustomDate? _Date = null;
		public new CustomDate? Date { get => _Date ?? base.Date; set => _Date = value; }

		public Holiday(string name, CustomDate? date) : base(name, date, EventType.Holiday)
		{
		}

		public override string Describe(int year)
		{
			var desc = Identifier;
			var age = Date?.AgeIn(year);

			if (age.HasValue)
			{
				desc = age.Value.AsOrdinal() + " annual " + desc;
			}

			return desc;
		}
	}
}
