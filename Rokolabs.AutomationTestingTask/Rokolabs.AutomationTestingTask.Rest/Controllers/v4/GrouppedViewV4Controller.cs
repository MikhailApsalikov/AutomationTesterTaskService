using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rokolabs.AutomationTestingTask.Common;
using Rokolabs.AutomationTestingTask.Entities;
using Rokolabs.AutomationTestingTask.Entities.Enums;
using Rokolabs.AutomationTestingTask.Entities.Filters;
using Rokolabs.AutomationTestingTask.Rest.Models;

namespace Rokolabs.AutomationTestingTask.Rest.Controllers.v4
{
	[Route("v4/GrouppedView")]
	public class GrouppedViewV4Controller : ApiController
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
			filter.PageSize = int.MaxValue;
			var events = repository.GetByFilter(filter);
			IEnumerable<EventGroup> result;
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
				case GroupBy.InteractionType:
					result = GroupByInteractionType(events);
					break;
				default:
					return InternalServerError(new ArgumentException("GroupBy"));
			}
			return Ok(result.ToList());
		}

		[NonAction]
		private static IEnumerable<EventGroup> GroupByAddressType(List<Event> events)
		{
			return events.GroupBy(g => g.AddressType.ToString())
				.Select(s => new EventGroup()
				{
					Events = s.ToList(),
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
			});
		}

		[NonAction]
		private static IEnumerable<EventGroup> GroupByBroker(List<Event> events)
		{
			return events.GroupBy(g => g.Broker).Select(s => new EventGroup()
			{
				Events = s.ToList(),
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