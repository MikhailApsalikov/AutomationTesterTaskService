using System.Web.Http;
using System.Linq;
using Rokolabs.AutomationTestingTask.Common;
using Rokolabs.AutomationTestingTask.Entities;
using Rokolabs.AutomationTestingTask.Entities.Filters;
using Rokolabs.AutomationTestingTask.Rest.Models;

namespace Rokolabs.AutomationTestingTask.Rest.Controllers.v4
{
	[Route("v4/Summary")]
	public class SummaryV4Controller : ApiController
	{
		[HttpGet]
		public IHttpActionResult Get([FromUri]EventFilter filter)
		{
			DelayHelper.LongDelay();
			var repository = EventRepositoryCache.Instance.Get(filter.SessionId.ToGuidWithAccessDenied());
			if (filter?.GroupBy != null)
			{
				return InternalServerError();
			}
			var events = repository.GetByFilter(filter);
			var result = new Summary()
			{
				Count = events.Count,
				EventCount = events.Count(e => !repository.IsInteraction(e)),
				InteractionCount = events.Count(e => repository.IsInteraction(e)),
				MostPopularBroker = events.GroupBy(e => e.Broker).OrderByDescending(s => s.Key).FirstOrDefault()?.Key,
				AverageDuration = events.Average(e => e.Duration)
			};
			return Ok(result);
		}
	}
}