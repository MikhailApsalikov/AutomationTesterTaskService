using System;
using System.Collections.Generic;
using System.Security.Authentication;
using Rokolabs.AutomationTestingTask.Repositories;
using Rokolabs.AutomationTestingTask.Rest.AccountService;

namespace Rokolabs.AutomationTestingTask.Rest.Models
{
	public class EventRepositoryCache
	{
		private const string AccountServiceAddress = "";

		public static EventRepositoryCache Instance { get; set; } = new EventRepositoryCache();
		private readonly Dictionary<Guid, EventRepository> repositories = new Dictionary<Guid, EventRepository>();

		private EventRepositoryCache()
		{
			
		}

		public EventRepository Get(Guid sessionId)
		{
			if (!CheckLoggedIn(sessionId))
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

		private bool CheckLoggedIn(Guid sessionId)
		{
			AccountServiceClient client = new AccountServiceClient();
			return client.CheckLoggedIn(sessionId);
		}
	}
}