using Dates;
using Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Events
{
	public class Observance : CalendarEvent
	{
		private CustomDate? _Date;

		[NotNull]
		public new CustomDate Date
		{ 
			get => _Date;
			set => _Date = value;
		}

		public Observance([NotNull] string name, [NotNull] CustomDate date) : base(name, date, EventType.Observance)
		{
			_Date = base.Date;
		}

		public override string Describe(int year)
		{
			var desc = Identifier;
			var age = Date?.AgeIn(year);

			if (age.HasValue)
			{
				desc = (age.Value + 1).AsOrdinal() + " annual " + desc;
			}

			return desc;
		}
	}
}
