using Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Dates
{
	public class FloatingDate : CustomDate
	{
		public FloatingDate(int month, int instance, int dayOfWeek, int? offset = 0)
			: this(month, instance, DayOfWeek.Sunday, offset)
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dayOfWeek);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(dayOfWeek, 7);
			DayOfWeek = IntToDOWMapping[dayOfWeek];
		}

		public FloatingDate(int month, int instance, DayOfWeek dayOfWeek, int? offset = 0)
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(month);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(month, 12);
			Month = month;

			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(instance);
			Instance = instance;

			DayOfWeek = dayOfWeek;

			Offset = offset;
		}

		public int Month { get; init; }
		public int Instance { get; init; }
		public DayOfWeek DayOfWeek { get; init; }

		public static new Regex RegEx = new Regex($"^(?<{nameof(Month)}>\\d{{2}})-N(?<{nameof(Instance)}>\\d)-(?<{nameof(DayOfWeek)}>\\d)(_(?<{nameof(Offset)}>[+-]?\\d{{1,3}}))?$", RegexOptions.ExplicitCapture & RegexOptions.Compiled);

		public static new FloatingDate Parse(string s, IFormatProvider? provider = null)
		{
			ArgumentNullException.ThrowIfNull(s);
			if (!RegEx.IsMatch(s)) throw new FormatException();

			provider ??= CultureInfo.CurrentCulture;

			var groups = RegEx.Match(s).Groups;

			var month = groups[nameof(Month)].Value;
			var instance = groups[nameof(Instance)].Value;
			var dayOfWeek = groups[nameof(DayOfWeek)].Value;
			var offset = groups[nameof(Offset)].Value;

			if (offset == string.Empty) offset = null;

			try
			{
				return new FloatingDate(
					int.Parse(month),
					int.Parse(instance),
					int.Parse(dayOfWeek),
					offset != null ? int.Parse(offset) : null
				);
			}
			catch (ArgumentException e)
			{
				throw new OverflowException("Failed to parse.", e);
			}
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out FloatingDate result)
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
			date = date.AddDays((Instance - 1) * 7).AddDays(Offset ?? 0);

			return date;
		}
	}
}
