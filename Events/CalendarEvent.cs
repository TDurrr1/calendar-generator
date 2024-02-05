using Dates;

namespace Events
{
    public enum EventType
    {
        Birthday,
        Death,
        Marriage,
        Holiday
    }

    public abstract class CalendarEvent
    {
        public string Identifier { get; init; }
        public CustomDate Date { get; init; }
        public EventType Type { get; init; }

        public CalendarEvent(string identifier, CustomDate date, EventType type)
        {
            if (identifier == null) throw new ArgumentNullException(nameof(identifier));
            identifier = identifier.Trim();
            if (identifier.Length == 0) throw new ArgumentException("Identifier is blank.", nameof(identifier));
            Identifier = identifier;

            if (date == null) throw new ArgumentNullException(nameof(date));
            Date = date;

            Type = type;
        }

        public abstract string GenerateDescription(int inYear);
    }
}
