﻿using Events;

namespace State
{
	public interface IAppState
	{
		public string EventCsvData { set; }
		public IEnumerable<CalendarEvent> Events { get; }

		public event Action<string>? OnEventCsvDataChanged;
	}

	public class AppState : IAppState
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
