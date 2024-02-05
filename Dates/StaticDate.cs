using Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Dates
{
    public class StaticDate : CustomDate
    {
        public StaticDate(int? year, int month, int day, int? offset = 0, LeapDayAdjustment lda = LeapDayAdjustment.March1)
        {
            if (year.HasValue && year <= 0) throw new ArgumentOutOfRangeException(nameof(year), "Year must be a whole number, if specified.");
            Year = year;

            if (!month.InRange(1, 12)) throw new ArgumentOutOfRangeException(nameof(month), "Month must be a value between 1 and 12, inclusive.");
            Month = month;

            var maxDay = new DateOnly(
                (year ?? 2000) + (month == 12 ? 1 : 0),
                month == 12 ? 1 : month + 1,
                1
            ).AddDays(-1).Day;
            if (!day.InRange(1, maxDay)) throw new ArgumentOutOfRangeException(nameof(month), "For specified year and month, day must be a value between 1 and " + maxDay + ", inclusive.");
            Day = day;

            Offset = offset;

            LeapDayAdjustment = lda;
        }

        public int Month { get; init; }
        public int Day { get; init; }
        public LeapDayAdjustment LeapDayAdjustment { get; init; }

        public static new Regex RegEx = new Regex($"^((?<{nameof(Year)}>\\d{{4}})-)?(?<{nameof(Month)}>\\d{{2}})-(?<{nameof(Day)}>\\d{{2}})(_(?<{nameof(Offset)}>[+-]?\\d{{1,3}}))?$", RegexOptions.ExplicitCapture & RegexOptions.Compiled);

        public static new StaticDate Parse(string s, IFormatProvider? provider = null)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
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
                    offset != null ? int.Parse(offset) : null,
                    LeapDayAdjustment.ThrowException
                );
            }
            catch(ArgumentException e)
            {
                throw new OverflowException("Failed to parse.", e);
            }
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out StaticDate result)
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
            if (inYear <= 0) throw new ArgumentOutOfRangeException(nameof(inYear), "Year must be a whole number.");

            var month = Month;
            var day = Day;
            if(Month == 2 && Day == 29 && !DateTime.IsLeapYear(inYear))
            {
                switch(LeapDayAdjustment)
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

            return new DateOnly(inYear, month, day).AddDays(Offset ?? 0);
        }
    }

    public enum LeapDayAdjustment
    {
        February28,
        March1,
        ThrowException
    }
}