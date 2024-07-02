using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Dates
{
	public class FloatingDate : CustomDate, IParsable<FloatingDate>
	{
		public int Month { get; protected init; }
		public int Instance { get; protected init; }
		public DayOfWeek DayOfWeek { get; protected init; }

		public FloatingDate(int? year, int month, int instance, int dayOfWeek, int offset = 0) : this(year, month, instance, DayOfWeek.Sunday, offset)
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dayOfWeek);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(dayOfWeek, 7);
			DayOfWeek = IntToDOWMapping[dayOfWeek];
		}

		public FloatingDate(int? year, int month, int instance, DayOfWeek dayOfWeek, int offset = 0) : base(year, offset)
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(month);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(month, 12);
			Month = month;

			ArgumentOutOfRangeException.ThrowIfLessThan(instance, -1);
			ArgumentOutOfRangeException.ThrowIfZero(instance);
			Instance = instance;

			DayOfWeek = dayOfWeek;
		}

		public FloatingDate(int month, int instance, int dayOfWeek, int offset = 0) : this(null, month, instance, dayOfWeek, offset)
		{
		}

		public FloatingDate(int month, int instance, DayOfWeek dayOfWeek, int offset = 0) : this(null, month, instance, dayOfWeek, offset)
		{
		}

		public static Regex RegEx = new Regex($"^((?<{nameof(Year)}>\\d{{4}})-)?(?<{nameof(Month)}>\\d{{2}})-N(?<{nameof(Instance)}>\\d|L)-(?<{nameof(DayOfWeek)}>\\d)(_(?<{nameof(Offset)}>[+-]?\\d{{1,3}}))?$", RegexOptions.ExplicitCapture & RegexOptions.Compiled);

		public static new FloatingDate Parse([DisallowNull] string s, IFormatProvider? provider)
		{
			if (!RegEx.IsMatch(s)) throw new FormatException();

			provider ??= CultureInfo.CurrentCulture;

			var groups = RegEx.Match(s).Groups;

			var year = groups[nameof(Year)].Value;
			var month = groups[nameof(Month)].Value;
			var instance = groups[nameof(Instance)].Value;
			var dayOfWeek = groups[nameof(DayOfWeek)].Value;
			var offset = groups[nameof(Offset)].Value;

			if (year == string.Empty) year = null;
			if (instance == "L") instance = "-1";
			if (offset == string.Empty) offset = null;

			try
			{
				return new FloatingDate(
					year != null ? int.Parse(year) : null,
					int.Parse(month),
					int.Parse(instance),
					int.Parse(dayOfWeek),
					offset != null ? int.Parse(offset) : 0
				);
			}
			catch (ArgumentException e)
			{
				throw new OverflowException("Failed to parse.", e);
			}
		}

		public static bool TryParse(string? s, IFormatProvider? provider, [NotNullWhen(true), MaybeNullWhen(false)] out FloatingDate result)
		{
			provider ??= CultureInfo.CurrentCulture;
			result = null;

			try
			{
				result = Parse(s, provider);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static readonly IReadOnlyDictionary<int, DayOfWeek> IntToDOWMapping = new Dictionary<int, DayOfWeek>
		{
			{ 1, DayOfWeek.Sunday },
			{ 2, DayOfWeek.Monday },
			{ 3, DayOfWeek.Tuesday },
			{ 4, DayOfWeek.Wednesday },
			{ 5, DayOfWeek.Thursday },
			{ 6, DayOfWeek.Friday },
			{ 7, DayOfWeek.Saturday }
		};

		public override DateOnly CalculateDate(int inYear)
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(inYear);

			DateOnly date = new DateOnly(inYear, Month, 1);
			while (date.DayOfWeek != DayOfWeek)
			{
				date = date.AddDays(1);
			}

			if (Instance != -1)
			{
				date = date.AddDays((Instance - 1) * 7).AddDays(Offset);
			}
			else
			{
				do
				{
					date = date.AddDays(7);
				} while (date.AddDays(7).Month == date.Month);
			}

			return date;
		}
	}
}
