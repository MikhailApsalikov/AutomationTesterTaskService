using System;
using System.Collections.Generic;
using System.Linq;
using Rokolabs.AutomationTestingTask.Repositories;
using Rokolabs.AutomationTestingTask.Soap.Models;

namespace Rokolabs.AutomationTestingTask.Soap
{
	public class AccountService : IAccountService
	{
		private const string SecretKey = "ROKO University";

		public AccountRepository AccountRepository { get; set; } = new AccountRepository();

		public bool CheckLoggedIn(Guid sessionId)
		{
			return AccountRepository.CheckLoggedIn(sessionId);
		}

		public string GetApiVersion()
		{
			return "Version 2.0";
		}

		public AccountModel GetUser(string secretKey, string username)
		{
			if (secretKey == SecretKey)
			{
				var account = AccountRepository.Get(username);
				if (account == null)
				{
					return null;
				}
				return new AccountModel()
				{
					Id = account.Id,
					Login = account.Login,
					Password = account.Password
				};
			}
			return null;
		}

		public List<AccountModel> GetUsers(string secretKey, int page, int pageSize)
		{
			if (secretKey == SecretKey)
			{
				page = page > 0 ? page : 1;
				pageSize = pageSize > 0 ? pageSize : 20;
				return AccountRepository.Get(page, pageSize).Select(a=>new AccountModel()
				{
					Id = a.Id,
					Login = a.Login,
					Password = a.Password
				}).ToList();
			}
			return null;
		}
	}
}
