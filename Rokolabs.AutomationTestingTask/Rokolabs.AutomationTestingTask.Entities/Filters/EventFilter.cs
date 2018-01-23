using System;
using Rokolabs.AutomationTestingTask.Entities.Enums;

namespace Rokolabs.AutomationTestingTask.Entities.Filters
{
	public class EventFilter
	{
		public GroupBy? GroupBy { get; set; }

		public DateTime DateFrom { get; set; }

		public DateTime DateTo { get; set; }

		public string Broker { get; set; }

		public InteractionTypes InteractionType { get; set; }

		public MeetingTypes MeetingType { get; set; }

		public string Title { get; set; }

		public string Location { get; set; }

		public string Company { get; set; }

		public AddressTypes AddressType { get; set; }

		public int Page { get; set; }
		public int PageSize { get; set; }
	}
}
