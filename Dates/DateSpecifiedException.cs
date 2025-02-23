namespace Dates
{
	public class DateSpecifiedException : InvalidOperationException
	{
		public DateSpecifiedException() : base("A date has already been specified for this event.") { }
	}
}
