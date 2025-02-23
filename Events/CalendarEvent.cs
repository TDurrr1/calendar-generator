using Dates;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace Events
{
	public enum EventType
	{
		Birth,
		Death,
		Marriage,
		Observance
	}

	public abstract class CalendarEvent
	{
		private static readonly FrozenDictionary<CalendarVersionName, string> EmptyVersionIdentifiers = new Dictionary<CalendarVersionName, string>().ToFrozenDictionary();

		private string _DefaultIdentifier;
		public string DefaultIdentifier
		{
			get => _DefaultIdentifier;

			[MemberNotNull(nameof(_DefaultIdentifier))]
			protected init
			{
				ArgumentNullException.ThrowIfNull(value);
				ArgumentException.ThrowIfNullOrWhiteSpace(value);
				_DefaultIdentifier = value;
			}
		}

		private FrozenDictionary<CalendarVersionName, string> _VersionIdentifiers;
		public FrozenDictionary<CalendarVersionName, string> VersionIdentifiers
		{
			get => _VersionIdentifiers;

			[MemberNotNull(nameof(_VersionIdentifiers))]
			protected init
			{
				ArgumentNullException.ThrowIfNull(value);

				var identifiers = new Dictionary<CalendarVersionName, string>();
				foreach (var kvp in value)
				{
					ArgumentException.ThrowIfNullOrWhiteSpace(kvp.Value);
					identifiers.Add(kvp.Key, kvp.Value.Trim());
				}

				_VersionIdentifiers = identifiers.ToFrozenDictionary();
			}
		}

		private string? _CalculationDescription;
		private string? CalculationDescription
		{
			get
			{
				if (!DateSpecified) return _CalculationDescription;
				throw new DateSpecifiedException();
			}

			init
			{
				ArgumentNullException.ThrowIfNull(value);
				ArgumentException.ThrowIfNullOrWhiteSpace(value);

				if (DateSpecified) throw new DateSpecifiedException();
				_CalculationDescription = value.Trim();
			}
		}

		private CustomDate? _Date;
		public CustomDate Date
		{
			get
			{
				if (DateSpecified) return _Date;
				throw new DateUnspecifiedException(CalculationDescription);
			}

			[MemberNotNull(nameof(_Date))]
			set
			{
				ArgumentNullException.ThrowIfNull(value);
				if (DateSpecified)
				{
					throw new DateSpecifiedException();
				}

				_Date = value;
				_CalculationDescription = null;
			}
		}

		public readonly bool IgnoreAge = false;

		/// <summary>
		/// This is only here so that the compiler doesn't complain about the Date getter.
		/// </summary>
		[MemberNotNullWhen(true, nameof(_Date))]
		[MemberNotNullWhen(false, nameof(CalculationDescription))]
		private bool DateSpecified => _Date is not null;

		public virtual EventType Type { get; protected init; }

		public CalendarEvent(EventType type, string identifier, CustomDate date, IDictionary<CalendarVersionName, string>? versionIdentifiers = null, bool ignoreAge = false)
		{
			DefaultIdentifier = identifier;
			Date = date;
			Type = type;
			VersionIdentifiers = (versionIdentifiers ?? EmptyVersionIdentifiers).ToFrozenDictionary();
			IgnoreAge = ignoreAge;
		}

		public CalendarEvent(EventType type, string identifier, string calculationDescription, IDictionary<CalendarVersionName, string>? versionIdentifiers = null, bool ignoreAge = false)
		{
			DefaultIdentifier = identifier;
			CalculationDescription = calculationDescription;
			Type = type;
			VersionIdentifiers = (versionIdentifiers ?? EmptyVersionIdentifiers).ToFrozenDictionary();
			IgnoreAge = ignoreAge;
		}

		public abstract string Describe(int inYear, CalendarVersionName version);
	}
}
