using System.Collections.Generic;

namespace Rokolabs.AutomationTestingTask.Entities
{
	public class EventList
	{
		public List<Event> Items { get; set; }

		public EventList()
		{

		}

		public EventList(List<Event> items)
		{
			this.Items = items;
		}
	}
}
