using System.Web.Http;
using System.Linq;
using Rokolabs.AutomationTestingTask.Common;
using Rokolabs.AutomationTestingTask.Entities;
using Rokolabs.AutomationTestingTask.Entities.Filters;
using Rokolabs.AutomationTestingTask.Rest.Models;

namespace Rokolabs.AutomationTestingTask.Rest.Controllers.v3
{
	[Route("v3/Summary")]
	public class SummaryV3Controller : ApiController
	{
		[HttpGet]
		public IHttpActionResult Get([FromUri]EventFilter filter)
		{
			var repository = EventRepositoryCache.Instance.Get(filter.SessionId.ToGuidWithAccessDenied());
			if (filter?.GroupBy != null)
			{
				return InternalServerError();
			}
			var events = repository.GetByFilter(filter);
			var result = new Summary()
			{
				Count = events.Count,
				EventCount = events.Count(e => !repository.IsInteraction(e)) + 1,
				InteractionCount = events.Count(e => repository.IsInteraction(e)),
				MostPopularBroker = events.Any() ? events.GroupBy(e => e.Broker).OrderByDescending(s => s.Key).FirstOrDefault()?.Key : null,
				AverageDuration = events.Any() ? events.Average(e => e.Duration) : 0
			};

			return Ok(result);
		}
	}
}