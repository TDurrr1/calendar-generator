using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Dates
{
	public abstract class CustomDate : IParsable<CustomDate>
	{
		public int? Year { get; init; }
		public int? Offset { get; init; }

		public int? AgeIn(int inYear)
		{
			if (!Year.HasValue) return null;
			return inYear - Year.Value;
		}

		public abstract DateOnly CalculateDate(int inYear);

		// TylerTODO: Implement these
		public static Regex RegEx { get { throw new NotImplementedException(); } }

		public static CustomDate Parse(string s, IFormatProvider? provider)
		{
			throw new NotImplementedException();
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out CustomDate result)
		{
			throw new NotImplementedException();
		}
	}

}
