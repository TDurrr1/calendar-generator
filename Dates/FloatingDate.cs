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
            if (!dayOfWeek.InRange(1, 7)) throw new ArgumentOutOfRangeException(nameof(dayOfWeek), "Day of week must be a value between 1 and 7, inclusive.");
            DayOfWeek = DayOfWeekIntToEnum(dayOfWeek);
        }

        public FloatingDate(int month, int instance, DayOfWeek dayOfWeek, int? offset = 0)
        {
            if (!month.InRange(1, 12)) throw new ArgumentOutOfRangeException(nameof(month), "Month must be a value between 1 and 12, inclusive.");
            Month = month;

            if (instance <= 0) throw new ArgumentOutOfRangeException(nameof(instance), "Instance must be a whole number.");
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
            if (s == null) throw new ArgumentNullException(nameof(s));
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
                    DayOfWeekIntToEnum(int.Parse(dayOfWeek)),
                    offset != null ? int.Parse(offset) : null
                );
            }
            catch(ArgumentException e)
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

        private static DayOfWeek DayOfWeekIntToEnum(int dow)
        {
            switch (dow)
            {
                case 1: return DayOfWeek.Sunday;
                case 2: return DayOfWeek.Monday;
                case 3: return DayOfWeek.Tuesday;
                case 4: return DayOfWeek.Wednesday;
                case 5: return DayOfWeek.Thursday;
                case 6: return DayOfWeek.Friday;
                case 7: return DayOfWeek.Saturday;
            }
            throw new ArgumentOutOfRangeException(nameof(dow), "Value must be between 1 and 7, inclusive");
        }

        public override DateOnly CalculateDate(int inYear)
        {
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
