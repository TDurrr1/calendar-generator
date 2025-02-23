namespace Dates
{
	public class DateUnspecifiedException : InvalidOperationException
	{
		public DateUnspecifiedException(string message) : base(message) { }
	}
}
