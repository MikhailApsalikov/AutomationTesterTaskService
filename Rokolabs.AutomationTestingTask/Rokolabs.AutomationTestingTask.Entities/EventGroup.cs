using System.Collections.Generic;

namespace Rokolabs.AutomationTestingTask.Entities
{
	public class EventGroup
	{
		public string GroupName { get; set; }
		public List<Event> Events { get; set; }
	}
}
