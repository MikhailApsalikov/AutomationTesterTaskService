using System.ServiceModel;
using Rokolabs.AutomationTestingTask.Soap.Models;

namespace Rokolabs.AutomationTestingTask.Soap
{
	[ServiceContract]
	public interface IAccounts
	{
		[OperationContract]
		string Login(string email, string password);

		[OperationContract]
		bool Logout(string sessionId);

		[OperationContract]
		AccountModel Registrate(string email, string password);

		[OperationContract]
		AccountModel GetUserBySessionId(string sessionId);
	}
}
