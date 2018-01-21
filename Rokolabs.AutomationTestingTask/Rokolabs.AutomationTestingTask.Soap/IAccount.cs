using System;
using System.ServiceModel;

namespace Rokolabs.AutomationTestingTask.Soap
{
	[ServiceContract]
	public interface IAccountService
	{
		[OperationContract]
		bool CheckLoggedIn(Guid sessionId);
	}
}
