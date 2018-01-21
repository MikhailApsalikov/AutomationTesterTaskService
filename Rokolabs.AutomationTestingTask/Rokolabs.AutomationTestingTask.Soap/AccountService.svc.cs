using System;
using Rokolabs.AutomationTestingTask.Repositories;

namespace Rokolabs.AutomationTestingTask.Soap
{
	public class AccountService : IAccountService
	{
		public AccountRepository AccountRepository { get; set; } = new AccountRepository();

		public bool CheckLoggedIn(Guid sessionId)
		{
			return AccountRepository.CheckLoggedIn(sessionId);
		}
	}
}
