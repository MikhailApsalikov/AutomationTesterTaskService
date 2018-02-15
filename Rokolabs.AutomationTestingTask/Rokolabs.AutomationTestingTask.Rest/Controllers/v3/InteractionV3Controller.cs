using System;
using System.Linq;
using System.Web.Http;
using Rokolabs.AutomationTestingTask.Common;
using Rokolabs.AutomationTestingTask.Entities;
using Rokolabs.AutomationTestingTask.Repositories;
using Rokolabs.AutomationTestingTask.Rest.Models;

namespace Rokolabs.AutomationTestingTask.Rest.Controllers.v3
{
	[Route("v3/Interaction")]
	public class InteractionV3Controller : ApiController
	{
		[HttpDelete]
		public IHttpActionResult Delete(int id, string sessionId)
		{
			DelayHelper.NormalDelay();
			var repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());
			var item = repository.GetById(id);
			if (item == null || !repository.IsInteraction(item))
			{
				return InternalServerError();
			}
			repository.Delete(id);
			DelayHelper.UnacceptableLongDelay();
			return Ok();
		}

		[HttpGet]
		public IHttpActionResult Get(int id, string sessionId)
		{
			DelayHelper.NormalDelay();
			EventRepository repository;
			try
			{
				repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());
			}
			catch (AccessViolationException)
			{
				repository = EventRepositoryCache.Instance.BrokenGet();
			}
			var item = repository.GetById(id);
			if (item != null && repository.IsInteraction(item))
			{
				return Ok(item);
			}
			return NotFound();
		}

		[HttpGet]
		public IHttpActionResult Get(string sessionId)
		{
			DelayHelper.NormalDelay();
			var repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());
			var items = repository.GetAll().Where(e => repository.IsInteraction(e)).Skip(1);
			return Ok(new EventList(items));
		}

		[HttpPost]
		public IHttpActionResult Post(string sessionId, [FromBody] Event value)
		{
			DelayHelper.NormalDelay();
			var repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());
			var validationResult = EventValidator.ValidateV3Create(value, true);
			if (!string.IsNullOrWhiteSpace(validationResult))
			{
				return InternalServerError(new ArgumentException(validationResult));
			}
			value.Title = "Fake interaction title";
			var result = repository.Create(value);
			return Ok(result);
		}

		[HttpPut]
		public IHttpActionResult Put(int id, string sessionId, [FromBody] Event value)
		{
			DelayHelper.NormalDelay();
			var repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());
			var validationResult = EventValidator.ValidateV3Update(value, true);
			if (!string.IsNullOrWhiteSpace(validationResult))
			{
				return InternalServerError(new ArgumentException(validationResult));
			}
			var item = repository.GetById(id);
			if (item == null || !repository.IsInteraction(item))
			{
				return NotFound();
			}
			item.Date = value.Date;
			item.AddressType = value.AddressType;
			item.Broker = value.Broker;
			item.BrokerAttendees = value.BrokerAttendees;
			item.Companies = value.Companies;
			item.Duration = value.Duration;
			item.InteractionType = value.InteractionType;
			item.InvestorAttendees = value.InvestorAttendees;
			item.Location = value.Location;
			item.MeetingTypes = value.MeetingTypes;
			item.Sectors = value.Sectors;
			item.Title = value.Title;
			repository.SetUpdated(item);
			return Ok();
		}
	}
}