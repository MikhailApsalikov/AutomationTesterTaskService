using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Rokolabs.AutomationTestingTask.Entities;
using Rokolabs.AutomationTestingTask.Entities.Enums;
using Rokolabs.AutomationTestingTask.Entities.Filters;

namespace Rokolabs.AutomationTestingTask.Repositories
{
	public class EventRepository
	{
		private List<Event> events = new List<Event>();

		public EventRepository()
		{
			InitEvents();
		}

		public List<Event> GetByFilter(EventFilter filter)
		{
			NormalizeFilter(filter);
			IEnumerable<Event> result = events;
			if (string.IsNullOrWhiteSpace(filter.Location))
			{
				result = result.Where(e =>
					e.Location != null && (e.Location.City.Contains(filter.Location) || e.Location.Country.Contains(filter.Location)));
			}

			if (filter.Sort != null && string.IsNullOrWhiteSpace(filter.SortBy))
			{
				result = Sort(result, filter.Sort.Value, filter.SortBy);
			}

			return result
				.Skip((filter.Page - 1) * filter.PageSize)
				.Take(filter.PageSize)
				.ToList();
		}

		public Event GetById(int id)
		{
			return events.FirstOrDefault(e => e.EventId == id);
		}

		public int Create(Event newEvent)
		{
			var id = events.Max(e => e.EventId);
			newEvent.EventId = id;
			newEvent.Created = DateTime.Now;
			events.Add(newEvent);
			return id;
		}

		public void SetUpdated(Event newEvent)
		{
			newEvent.Updated = DateTime.Now;
		}

		public void Delete(int id)
		{
			events = events.Where(e => e.EventId != id).ToList();
		}

		#region Initialization default data

		private void InitEvents()
		{
			events.Add(new Event
			{
				Location = new Location
				{
					Country = "Russia",
					City = "Saratov"
				},
				AddressType = AddressTypes.Home,
				Broker = "Rokolabs",
				BrokerAttendees = new[]
				{
					new User
					{
						Id = 1000001,
						Username = "Vasya"
					},
					new User
					{
						Id = 1000002,
						Username = "Petya"
					}
				},
				Companies = new List<Company>
				{
					new Company
					{
						Id = 115,
						Name = "Rokolabs",
						Ticker = "ROKO"
					}
				},
				Created = DateTime.Now.AddDays(-1),
				Duration = 1000,
				EventId = 1,
				InteractionType = InteractionTypes.Conference,
				InvestorAttendees = new List<User>
				{
					new User
					{
						Id = 1000003,
						Username = "Janetta"
					},
					new User
					{
						Id = 1000004,
						Username = "John"
					}
				},
				MeetingTypes = new List<MeetingTypes>()
				{
					MeetingTypes.Annotated,
					MeetingTypes.Im,
					MeetingTypes.OneByOne,
					MeetingTypes.Survey
				},
				Sectors = new List<Sectors>()
				{
					Sectors.Energy,
					Sectors.InformationTechnology,
					Sectors.Materials
				},
				Date = DateTime.Now.AddDays(1),
				Title = "Conference the first",
				Updated = DateTime.Now
			});
		}

		#endregion

		private void NormalizeFilter(EventFilter filter)
		{
			if (filter.Page == 0)
			{
				filter.Page = 1;
			}
			if (filter.PageSize == 0)
			{
				filter.PageSize = 20;
			}
		}

		private IEnumerable<Event> Sort(IEnumerable<Event> eventList, ListSortDirection filterSort, string filterSortBy)
		{
			switch (filterSortBy)
			{
				case "EventId":
					return (filterSort == ListSortDirection.Ascending) ? 
						eventList.OrderBy(e=>e.EventId) : 
						eventList.OrderByDescending(e => e.EventId);
				case "Date":
					return (filterSort == ListSortDirection.Ascending) ?
						eventList.OrderBy(e => e.Date) :
						eventList.OrderByDescending(e => e.Date);
				case "Broker":
					return (filterSort == ListSortDirection.Ascending) ?
						eventList.OrderBy(e => e.Broker) :
						eventList.OrderByDescending(e => e.Broker);
				case "InteractionType":
					return (filterSort == ListSortDirection.Ascending) ?
						eventList.OrderBy(e => e.InteractionType) :
						eventList.OrderByDescending(e => e.InteractionType);
				case "Title":
					return (filterSort == ListSortDirection.Ascending) ?
						eventList.OrderBy(e => e.Title) :
						eventList.OrderByDescending(e => e.Title);
				case "Duration":
					return (filterSort == ListSortDirection.Ascending) ?
						eventList.OrderBy(e => e.Duration) :
						eventList.OrderByDescending(e => e.Duration);
				case "AddressType":
					return (filterSort == ListSortDirection.Ascending) ?
						eventList.OrderBy(e => e.AddressType) :
						eventList.OrderByDescending(e => e.AddressType);
				case "Created":
					return (filterSort == ListSortDirection.Ascending) ?
						eventList.OrderBy(e => e.Created) :
						eventList.OrderByDescending(e => e.Created);
				case "Updated":
					return (filterSort == ListSortDirection.Ascending) ?
						eventList.OrderBy(e => e.Updated) :
						eventList.OrderByDescending(e => e.Updated);
			}
			return eventList;
		}
	}
}