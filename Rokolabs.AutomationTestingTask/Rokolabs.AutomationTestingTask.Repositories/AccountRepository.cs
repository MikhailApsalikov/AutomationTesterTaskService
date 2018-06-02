using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Security;
using Rokolabs.AutomationTestingTask.Entities;
using Rokolabs.AutomationTestingTask.Repositories.Context;

namespace Rokolabs.AutomationTestingTask.Repositories
{
	public class AccountRepository
	{
		public AutomationTaskDbContext DbContext { get; set; } = new AutomationTaskDbContext();

		public Account Create(Account account)
		{
			var result = DbContext.Accounts.Add(account);
			DbContext.SaveChanges();
			return result;
		}

		public bool CheckLoggedIn(Guid sessionId)
		{
			return DbContext.Accounts.Any(a => a.SessionUserId == sessionId);
		}

		public Account Get(Guid sessionId)
		{
			return DbContext.Accounts.FirstOrDefault(a => a.SessionUserId == sessionId);
		}

		public Account Get(string login)
		{
			return DbContext.Accounts.FirstOrDefault(a => a.Login == login);
		}

		public Account Get(string login, string password)
		{
			var account = DbContext.Accounts.FirstOrDefault(a => a.Login == login);
			if (account == null)
			{
				throw new ObjectNotFoundException("Login not found");
			}
			if (account.Password != password)
			{
				throw new ArgumentException("Incorrect password");
			}
			return account;
		}

		public Account Update(Account account)
		{
			var acc = DbContext.Accounts.Find(account.Id);
			acc.Login = account.Login;
			acc.Password = account.Password;
			acc.SessionUserId = account.SessionUserId;
			DbContext.Entry(acc).State = EntityState.Modified;
			DbContext.SaveChanges();
			return acc;
		}
	}
}
