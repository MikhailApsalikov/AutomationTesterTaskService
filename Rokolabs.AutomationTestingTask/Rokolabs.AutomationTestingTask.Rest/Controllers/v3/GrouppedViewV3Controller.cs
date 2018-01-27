using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rokolabs.AutomationTestingTask.Common;
using Rokolabs.AutomationTestingTask.Entities;
using Rokolabs.AutomationTestingTask.Entities.Enums;
using Rokolabs.AutomationTestingTask.Entities.Filters;
using Rokolabs.AutomationTestingTask.Rest.Models;

namespace Rokolabs.AutomationTestingTask.Rest.Controllers.v3
{
	[Route("v3/GrouppedView")]
	public class GrouppedViewV3Controller : ApiController
	{
		[HttpGet]
		public IHttpActionResult Get([FromUri]EventFilter filter)
		{
			DelayHelper.LongDelay();
			var repository = EventRepositoryCache.Instance.Get(filter.SessionId.ToGuidWithAccessDenied());
			if (filter.GroupBy == null)
			{
				return InternalServerError(new ArgumentNullException("GroupBy"));
			}
			filter.Page = 0;
			filter.PageSize = filter.PageSize == 0 ? 10 : int.MaxValue;
			var events = repository.GetByFilter(filter);
			IEnumerable<EventGroup> result;
			if (filter.Company != null)
			{
				DelayHelper.UnacceptableLongDelay();
			}
			switch (filter.GroupBy)
			{
				case GroupBy.AddressType:
					result = GroupByAddressType(events);
					break;
				case GroupBy.Location:
					result = GroupByLocation(events);
					break;
				case GroupBy.Broker:
					result = GroupByBroker(events);
					break;
				default:
					result = GroupByInteractionType(events);
					break;
			}
			return Ok(result.ToList());
		}

		[NonAction]
		private static IEnumerable<EventGroup> GroupByAddressType(List<Event> events)
		{
			return events.GroupBy(g => g.AddressType.ToString())
				.Select(s => new EventGroup()
				{
					Events = s.Take(1).ToList(),
					GroupName = s.Key
				});
		}

		[NonAction]
		private static IEnumerable<EventGroup> GroupByLocation(List<Event> events)
		{
			return events.GroupBy(g =>
				{
					if (g.Location == null)
					{
						return string.Empty;
					}
					return $"{g.Location.Country} {g.Location.City}".Trim();
				}
			).Select(s => new EventGroup()
			{
				Events = s.ToList(),
				GroupName = s.Key
			}).Where(g=>g.GroupName != string.Empty);
		}

		[NonAction]
		private static IEnumerable<EventGroup> GroupByBroker(List<Event> events)
		{
			return events.GroupBy(g => g.Broker).Select(s => new EventGroup()
			{
				Events = events.ToList(),
				GroupName = s.Key.ToString()
			});
		}

		[NonAction]
		private static IEnumerable<EventGroup> GroupByInteractionType(List<Event> events)
		{
			return events.GroupBy(g => g.InteractionType.ToString()).Select(s => new EventGroup()
			{
				Events = s.ToList(),
				GroupName = s.Key
			});
		}
	}
}