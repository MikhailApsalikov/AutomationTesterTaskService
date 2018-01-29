using System;
using System.Collections.Generic;
using Rokolabs.AutomationTestingTask.Entities.Enums;

namespace Rokolabs.AutomationTestingTask.Entities
{
	public class Event
	{
		public int EventId { get; set; }
		//datetime
		public DateTime Date { get; set; }
		//string
		public string Broker { get; set; }
		//enum
		public InteractionTypes InteractionType { get; set; }
		// list of enums
		public List<MeetingTypes> MeetingTypes { get; set; }
		// string
		public string Title { get; set; }
		// nested class without ID
		public Location Location { get; set; }
		// nested class with ID (List)
		public List<Company> Companies { get; set; }
		// nested account
		public List<User> InvestorAttendees { get; set; }
		// array
		public User[] BrokerAttendees { get; set; }
		// int
		public int Duration { get; set; }
		public List<Sectors> Sectors { get; set; }
		// flag enum
		public AddressTypes AddressType { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Updated { get; set; }
	}
}
