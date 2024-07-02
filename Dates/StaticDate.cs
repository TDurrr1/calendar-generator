using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Dates
{
	public class StaticDate : CustomDate, IParsable<StaticDate>
	{
		public StaticDate(int? year, int month, int day, int offset = 0, LeapDayAdjustment lda = LeapDayAdjustment.March1) : base(year, offset)
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(month);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(month, 12);
			Month = month;

			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(day);
			var maxDay = new DateOnly(
				 (year ?? 2000) + (month == 12 ? 1 : 0),
				 month == 12 ? 1 : month + 1,
				 1
			).AddDays(-1).Day;
			ArgumentOutOfRangeException.ThrowIfGreaterThan(day, maxDay);
			Day = day;

			LeapDayAdjustment = lda;
		}

		public int Month { get; protected init; }
		public int Day { get; protected init; }
		public LeapDayAdjustment LeapDayAdjustment { get; protected init; }

		public static Regex RegEx = new Regex($"^((?<{nameof(Year)}>\\d{{4}})-)?(?<{nameof(Month)}>\\d{{2}})-(?<{nameof(Day)}>\\d{{2}})(_(?<{nameof(Offset)}>[+-]?\\d{{1,3}}))?$", RegexOptions.ExplicitCapture & RegexOptions.Compiled);

		public static new StaticDate Parse([DisallowNull] string s, IFormatProvider? provider = null)
		{
			ArgumentNullException.ThrowIfNull(s);
			if (!RegEx.IsMatch(s)) throw new FormatException();

			provider ??= CultureInfo.CurrentCulture;

			var groups = RegEx.Match(s).Groups;

			var year = groups[nameof(Year)].Value;
			var month = groups[nameof(Month)].Value;
			var day = groups[nameof(Day)].Value;
			var offset = groups[nameof(Offset)].Value;

			if (year == string.Empty) year = null;
			if (offset == string.Empty) offset = null;

			try
			{
				return new StaticDate(
					 year != null ? int.Parse(year) : null,
					 int.Parse(month),
					 int.Parse(day),
					 offset != null ? int.Parse(offset) : 0,
					 LeapDayAdjustment.ThrowException
				);
			}
			catch (ArgumentException e)
			{
				throw new OverflowException("Failed to parse.", e);
			}
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [NotNullWhen(true), MaybeNullWhen(false)] out StaticDate result)
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

		public override DateOnly CalculateDate(int inYear)
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(inYear);

			var month = Month;
			var day = Day;
			if (Month == 2 && Day == 29 && !DateTime.IsLeapYear(inYear))
			{
				switch (LeapDayAdjustment)
				{
					case LeapDayAdjustment.February28:
						month = 2;
						day = 28;
						break;
					case LeapDayAdjustment.March1:
						month = 3;
						day = 1;
						break;
					case LeapDayAdjustment.ThrowException:
						throw new InvalidOperationException("Cannot generate date for February 29 in non-leap year.");
					default:
						throw new NotImplementedException("Case not specified for this LeapDayAdjustment value.");
				}
			}

			return new DateOnly(inYear, month, day).AddDays(Offset);
		}
	}

	public enum LeapDayAdjustment
	{
		February28,
		March1,
		ThrowException
	}
}