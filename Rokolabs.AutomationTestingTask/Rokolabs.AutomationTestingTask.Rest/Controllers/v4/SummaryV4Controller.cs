using System.Web.Http;
using Rokolabs.AutomationTestingTask.Entities.Filters;

namespace Rokolabs.AutomationTestingTask.Rest.Controllers.v4
{
	public class SummaryV4Controller : ApiController
	{
		[HttpGet]
		public IHttpActionResult Get(string sessionId, EventFilter filter)
		{
			return Ok();
		}
	}
}