using System;
using System.Collections.Generic;
using System.ServiceModel;
using Rokolabs.AutomationTestingTask.Soap.Models;

namespace Rokolabs.AutomationTestingTask.Soap
{
	[ServiceContract]
	public interface IAccountService
	{
		[OperationContract]
		bool CheckLoggedIn(Guid sessionId);

		[OperationContract]
		string GetApiVersion();

		[OperationContract]
		AccountModel GetUser(string secretKey, string username);

		[OperationContract]
		List<AccountModel> GetUsers(string secretKey, int page, int pageSize);
	}
}
