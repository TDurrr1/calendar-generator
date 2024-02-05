using Dates;

namespace Events
{
    public class Birthday : CalendarEvent
    {
        public string Name => Identifier;

        public Birthday(string name, CustomDate date) : base(name, date, EventType.Birthday)
        {
        }

        public override string GenerateDescription(int inYear)
        {
            throw new NotImplementedException();
        }
    }
}
