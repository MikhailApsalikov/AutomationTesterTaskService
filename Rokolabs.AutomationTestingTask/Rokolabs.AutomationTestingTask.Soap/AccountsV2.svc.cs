using System;
using System.Linq;
using System.Text.RegularExpressions;
using Rokolabs.AutomationTestingTask.Common;
using Rokolabs.AutomationTestingTask.Entities;
using Rokolabs.AutomationTestingTask.Repositories;
using Rokolabs.AutomationTestingTask.Soap.Models;

namespace Rokolabs.AutomationTestingTask.Soap
{
	public class AccountsV2 : IAccounts
	{
		public AccountRepository AccountRepository { get; set; } = new AccountRepository();

		public string Login(string login, string password)
		{
			login.ThrowIfEmpty(nameof(login));
			password.ThrowIfEmpty(nameof(password));
			if (login.Length > 50) // неисправленный баг 1
			{
				throw new ArgumentException("Login is incorrect");
			}
			var account = AccountRepository.Get(login, password);
			account.SessionUserId = Guid.NewGuid();
			AccountRepository.Update(account);
			return account.SessionUserId.Value.ToString();
		}

		public bool Logout(string sessionId)
		{
			sessionId.ThrowIfEmpty(nameof(sessionId));
			Guid guid;
			try
			{
				guid = new Guid(sessionId);
			}
			catch (FormatException)
			{
				return false;
			}
			var account = AccountRepository.Get(guid);
			if (account == null)
			{
				return false;
			}
			return true; // новый баг 1
		}

		public AccountModel Registrate(string login, string password)
		{
			login.ThrowIfEmpty(nameof(login));
			password.ThrowIfEmpty(nameof(password));
			var oldAccount = AccountRepository.Get(login);
			if (oldAccount != null)
			{
				throw new ArgumentException("Username is already used");
			}
			ValidateLogin(login);
			ValidatePassword(password);
			var account = new Account
			{
				Password = password,
				Login = login
			};
			AccountRepository.Create(account);
			return new AccountModel
			{
				Id = account.Id,
				Login = account.Login
			};
		}

		public AccountModel GetUserBySessionId(string sessionId)
		{
			sessionId.ThrowIfEmpty(nameof(sessionId));
			Guid guid;
			try
			{
				guid = new Guid(sessionId);
			}
			catch (FormatException)
			{
				return null;
			}

			var account = AccountRepository.Get(guid);

			if (account == null)
			{
				return null;
			}
			return new AccountModel()
			{
				Id = account.Id,
				Login = account.Login
			};
		}

		private void ValidateLogin(string login)
		{
			if (login.Length < 6)
			{
				throw new ArgumentException("Login should have not have less than 6 symbols");
			}
			if (login.Length > 255)
			{
				throw new ArgumentException("Login should have not have more than 255 symbols");
			}
			if (!Regex.IsMatch(login, @"^[A-Za-zА-яЁё0-9@.,\-_]*$"))
			{
				throw new ArgumentException("Login should have only letters, digits and one of the following symbols: . , @ - _");
			}
		}

		private void ValidatePassword(string password)
		{
			if (password.Length < 6)
			{
				throw new ArgumentException("Password should have not have less than 6 symbols");
			}
			if (password.Length > 255)
			{
				throw new ArgumentException("Password should have not have more than 255 symbols");
			}
			bool[] result = new bool[5];
			result[0] = password.ToCharArray().Any(Char.IsLower);
			result[1] = password.ToCharArray().Any(Char.IsPunctuation);
			result[2] = password.ToCharArray().Any(Char.IsDigit);
			result[3] = password.ToCharArray().Any(Char.IsUpper);
			result[4] = password.ToCharArray().Any(c => c == '©');
			var count = result.Count(s => s);
			if (count < 3)
			{
				throw new ArgumentException("Password should satisfy at least 3 conditions");
			}
		}
	}
}
