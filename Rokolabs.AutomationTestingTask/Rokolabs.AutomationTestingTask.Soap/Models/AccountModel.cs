using System.Runtime.Serialization;

namespace Rokolabs.AutomationTestingTask.Soap.Models
{
	[DataContract]
	public class AccountModel
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Login { get; set; }
		[DataMember]
		public string Password { get; set; }
	}
}