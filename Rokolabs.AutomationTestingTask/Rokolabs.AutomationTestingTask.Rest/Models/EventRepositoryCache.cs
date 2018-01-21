using System;
using System.Collections.Generic;
using System.Security.Authentication;
using Rokolabs.AutomationTestingTask.Repositories;

namespace Rokolabs.AutomationTestingTask.Rest.Models
{
	public class EventRepositoryCache
	{
		public static EventRepositoryCache Instance { get; set; }
		private readonly Dictionary<Guid, EventRepository> repositories = new Dictionary<Guid, EventRepository>();
		private AccountRepository AccountRepository { get; set; } = new AccountRepository();

		private EventRepositoryCache()
		{
			
		}

		public EventRepository Get(Guid sessionId)
		{
			if (!AccountRepository.CheckLoggedIn(sessionId))
			{
				throw new AuthenticationException("Invalid sessionId");
			}
			if (!repositories.ContainsKey(sessionId))
			{
				repositories.Add(sessionId, new EventRepository());
			}
			return repositories[sessionId];
		}

		public EventRepository BrokenGet()
		{
			return new EventRepository();
		}
	}
}