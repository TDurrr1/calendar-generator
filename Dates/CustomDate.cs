using System.Diagnostics.CodeAnalysis;

namespace Dates
{
	public abstract class CustomDate : IParsable<CustomDate>
	{
		public int? Year { get; protected init; }
		public int Offset { get; protected init; }

		protected CustomDate(int? year, int offset = 0)
		{
			if (year.HasValue) ArgumentOutOfRangeException.ThrowIfNegativeOrZero(year.Value);
			Year = year;

			Offset = offset;
		}

		protected CustomDate(int offset = 0): this(null, offset)
		{
		}

		public int? AgeIn(int inYear)
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(inYear);

			if (!Year.HasValue) return null;
			return inYear - Year.Value;
		}

		public abstract DateOnly CalculateDate(int inYear);

		public static CustomDate Parse(string s, IFormatProvider? provider)
		{
			if (IndeterminateDate.TryParse(s, provider, out IndeterminateDate indetDate)) return indetDate;
			if (FloatingDate.TryParse(s, provider, out FloatingDate floatingDate)) return floatingDate;
			if (StaticDate.TryParse(s, provider, out StaticDate staticDate)) return staticDate;
			throw new ArgumentException("Input string format was not recognized.", nameof(s));
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [NotNullWhen(true), MaybeNullWhen(false)] out CustomDate result)
		{
			result = null;
			try
			{
				result = Parse(s, provider);
				return true;
			}
			catch {
				return false;
			}
		}
	}

}
