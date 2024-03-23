using Dates;
using Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Events
{
	public class Holiday : CalendarEvent
	{
		private CustomDate? _Date;
		[NotNull]
		public new CustomDate Date { get => _Date ?? base.Date; set => _Date = value; }

		public Holiday([NotNull] string name, [NotNull] CustomDate date) : base(name, date, EventType.Holiday)
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
