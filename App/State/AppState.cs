using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarGenerator.State
{
    internal interface IAppState
    {
        public string EventCsvData { set; }
        public IEnumerable<CalendarEvent> Events { get; }

        public event Action<string>? OnEventCsvDataChanged;
    }

    internal class AppState : IAppState
    {
        public string EventCsvData
        {
            set
            {

                OnEventCsvDataChanged?.Invoke(value);
            }
        }

        public IEnumerable<CalendarEvent> Events { get; private set; }

        public event Action<string>? OnEventsFileChanged;
        public event Action<string>? OnEventCsvDataChanged;
    }
}
