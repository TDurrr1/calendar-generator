using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Dates
{
	public class IndeterminateDate : CustomDate, IParsable<IndeterminateDate>
	{
		public IndeterminateDate([NotNull] string note): base(null)
		{
			ArgumentNullException.ThrowIfNull(note);
			ArgumentException.ThrowIfNullOrWhiteSpace(note);
			Note = note;
		}

		public static Regex RegEx = new Regex($"^!(?<{nameof(Note)}>.+)$");
		public string Note { get; protected init; }

		public static new IndeterminateDate Parse([DisallowNull] string s, IFormatProvider? provider = null)
		{
			ArgumentNullException.ThrowIfNull(s);
			if (!s.StartsWith('!')) throw new FormatException();
			if (s.Length <= 1) throw new FormatException();

			try
			{
				return new IndeterminateDate(s[1..]);
			}
			catch (ArgumentException e)
			{
				throw new OverflowException("Failed to parse.", e);
			}
		}

		public static bool TryParse(string? s, IFormatProvider? provider, [NotNullWhen(true), MaybeNullWhen(false)] out IndeterminateDate result)
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
			throw new NotImplementedException("Cannot calculate indeterminate date");
		}
	}
}
