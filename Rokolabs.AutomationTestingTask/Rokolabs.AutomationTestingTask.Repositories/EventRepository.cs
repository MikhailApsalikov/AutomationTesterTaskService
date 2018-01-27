using System;
using System.Collections.Generic;
using System.Linq;
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
			NormalizeFilter(ref filter);
			IEnumerable<Event> result = events;
			if (!string.IsNullOrWhiteSpace(filter.Location))
			{
				result = result.Where(e =>
					e.Location != null && (e.Location.City.ToUpperInvariant().Contains(filter.Location.ToUpperInvariant())
					|| e.Location.Country.ToUpperInvariant().Contains(filter.Location.ToUpperInvariant())));
			}
			if (filter.DateFrom.HasValue)
			{
				result = result.Where(e => e.Date > filter.DateFrom);
			}
			if (filter.DateTo.HasValue)
			{
				result = result.Where(e => e.Date < filter.DateTo);
			}
			if (!string.IsNullOrWhiteSpace(filter.Broker))
			{
				result = result.Where(e => e.Broker.ToUpperInvariant().Contains(filter.Broker.ToUpperInvariant()));
			}
			if (!string.IsNullOrWhiteSpace(filter.Title))
			{
				result = result.Where(e => e.Title.ToUpperInvariant().Contains(filter.Title.ToUpperInvariant()));
			}
			if (!string.IsNullOrWhiteSpace(filter.Company))
			{
				result = result.Where(e => e.Companies != null && e.Companies.Any(s => s.Name.ToUpperInvariant().Contains(filter.Company.ToUpperInvariant())));
			}
			if (filter.InteractionType.HasValue)
			{
				result = result.Where(e => e.InteractionType == filter.InteractionType);
			}
			if (filter.AddressType.HasValue)
			{
				result = result.Where(e => e.AddressType == filter.AddressType);
			}
			if (filter.MeetingTypes != null && filter.MeetingTypes.Any())
			{
				result = result.Where(e => e.MeetingTypes != null && e.MeetingTypes.Any(s => filter.MeetingTypes.Any(d => d == s)));
			}
			if (filter.IsInteraction.HasValue)
			{
				result = filter.IsInteraction.Value ? 
					result.Where(IsInteraction) : 
					result.Where(e => !IsInteraction(e));
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
			int id = events.Max(e => e.EventId) + 1;
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

		private void NormalizeFilter(ref EventFilter filter)
		{
			if (filter == null)
			{
				filter = new EventFilter();
			}
			if (filter.Page == 0)
			{
				filter.Page = 1;
			}
			if (filter.PageSize == 0)
			{
				filter.PageSize = 20;
			}
		}
	}
}