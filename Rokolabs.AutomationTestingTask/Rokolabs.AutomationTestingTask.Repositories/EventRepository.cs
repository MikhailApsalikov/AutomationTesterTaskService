using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using Rokolabs.AutomationTestingTask.Common;
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

		public List<Event> GetAll()
		{
			return events;
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

		public bool IsInteraction(Event e)
		{
			return e.Date < DateTime.Now;
		}

		#region Initialization default data

		private void InitEvents()
		{
			List<Location> locations = new List<Location>
			{
				new Location
				{
					Country = "Russia",
					City = "Saratov"
				},
				new Location
				{
					Country = "Russia",
					City = "Samara"
				},
				new Location
				{
					Country = "USA",
					City = "New York"
				}
			};
			List<User> usersPool = new List<User>
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
				},
				new User
				{
					Id = 1000003,
					Username = "Janetta"
				},
				new User
				{
					Id = 1000004,
					Username = "John"
				},
				new User
				{
					Id = 1000005,
					Username = "Vovan"
				},
				new User
				{
					Id = 1000006,
					Username = "Angelica"
				},
				new User
				{
					Id = 1000007,
					Username = "Aleksandr"
				},
				new User
				{
					Id = 1000008,
					Username = "Gertruda"
				}
			};
			List<Company> companiesPool = new List<Company>()
			{
				new Company
				{
					Id = 115,
					Name = "Rokolabs",
					Ticker = "ROKO"
				},
				new Company
				{
					Id = 116,
					Name = "Google Inc",
					Ticker = "GGL"
				},
				new Company
				{
					Id = 117,
					Name = "McDonalds",
					Ticker = "MCDAC"
				},
				new Company
				{
					Id = 118,
					Name = "Vector",
					Ticker = "VCT"
				}
			};

			for (int i = 0; i < 10; i++)
			{
				events.Add(new Event
				{
					Location = locations[i % locations.Count],
					AddressType = (AddressTypes)(i % 4),
					Broker = "Rokolabs",
					BrokerAttendees = usersPool.Where(s => RandomGenerator.Instance.GenerateBool(0.3)).ToArray(),
					Companies = companiesPool.Where(s => RandomGenerator.Instance.GenerateBool(0.5)).ToList(),
					Created = DateTime.Now.AddDays(-i).AddHours(RandomGenerator.Instance.Generate(0, 17)),
					Duration = RandomGenerator.Instance.Generate(0, 10000),
					EventId = i + 1,
					InteractionType = (InteractionTypes)(i % 7),
					InvestorAttendees = usersPool.Where(s => RandomGenerator.Instance.GenerateBool(0.6)).ToList(),
					MeetingTypes = Enum.GetValues(typeof(MeetingTypes)).Cast<MeetingTypes>().Where(s => RandomGenerator.Instance.GenerateBool(0.2)).ToList(),
					Sectors = Enum.GetValues(typeof(Sectors)).Cast<Sectors>().Where(s => RandomGenerator.Instance.GenerateBool(0.3)).ToList(),
					Date = DateTime.Now.AddDays(i - 5),
					Title = $"Event {i + 1}",
					Updated = DateTime.Now
				});
			}
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
						eventList.OrderBy(e => e.EventId) :
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