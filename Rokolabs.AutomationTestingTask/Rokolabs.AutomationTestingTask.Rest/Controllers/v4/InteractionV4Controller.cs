using System.Web.Caching;
using System.Web.Http;
using Rokolabs.AutomationTestingTask.Entities;
using Rokolabs.AutomationTestingTask.Entities.Filters;

namespace Rokolabs.AutomationTestingTask.Rest.Controllers.v4
{
	[Route("v4/Interaction")]
	public class InteractionV4Controller : ApiController
	{
		

		[HttpDelete]
		public virtual IHttpActionResult Delete(int id, string sessionId)
		{
			return Ok();
		}

		[HttpGet]
		public virtual IHttpActionResult Get(int id, string sessionId)
		{
			
			return Ok();
		}

		[HttpGet]
		public virtual IHttpActionResult Get(string sessionId, [FromUri]EventFilter query)
		{
			return Ok();
		}

		[HttpPost]
		public virtual IHttpActionResult Post(string sessionId, [FromBody] Event value)
		{
			return Ok();
		}

		[HttpPut]
		public virtual IHttpActionResult Put(int id, string sessionId, [FromBody] Event value)
		{
			return Ok();
		}
	}
}