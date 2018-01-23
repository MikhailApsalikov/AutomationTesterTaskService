﻿using System;
using System.Linq;
using System.Web.Http;
using Rokolabs.AutomationTestingTask.Common;
using Rokolabs.AutomationTestingTask.Entities;
using Rokolabs.AutomationTestingTask.Entities.Enums;
using Rokolabs.AutomationTestingTask.Repositories;
using Rokolabs.AutomationTestingTask.Rest.Models;

namespace Rokolabs.AutomationTestingTask.Rest.Controllers.v2
{
	[Route("v2/Event")]
	public class EventV2Controller : ApiController
	{
		[HttpDelete]
		public IHttpActionResult Delete(int id, string sessionId)
		{
			var repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());
			var item = repository.GetById(id);
			if (item == null || repository.IsInteraction(item))
			{
				return NotFound();
			}
			repository.Delete(id);
			return Ok();
		}

		[HttpGet]
		public IHttpActionResult Get(int id, string sessionId)
		{
			var repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());
			var item = repository.GetAll().FirstOrDefault();
			if (item != null && !repository.IsInteraction(item))
			{
				return Ok(item);
			}
			return NotFound();
		}

		[HttpGet]
		public IHttpActionResult Get(string sessionId)
		{
			EventRepository repository;
			try
			{
				repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());
				var items = repository.GetAll().Where(e => !repository.IsInteraction(e));
				return Ok(items);
			}
			catch (AccessViolationException)
			{
				repository = EventRepositoryCache.Instance.BrokenGet();
			}
			return Ok();
		}

		[HttpPost]
		public IHttpActionResult Post(string sessionId, [FromBody] Event value)
		{
			var repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());
			var validationResult = EventValidator.ValidateV2Create(value, false);
			if (!string.IsNullOrWhiteSpace(validationResult))
			{
				return InternalServerError(new ArgumentException(validationResult));
			}
			value.InteractionType = InteractionTypes.Conference;
			var result = repository.Create(value);
			if (result % 5 == 0)
			{
				return Ok(result * 7);
			}
			return Ok(result);
		}

		[HttpPut]
		public IHttpActionResult Put(int id, string sessionId, [FromBody] Event value)
		{
			var repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());
			var validationResult = EventValidator.ValidateV2Update(value, false);
			if (!string.IsNullOrWhiteSpace(validationResult))
			{
				return InternalServerError(new ArgumentException(validationResult));
			}
			var item = repository.GetById(id);
			if (item == null || repository.IsInteraction(item))
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
			item.Title = value.Title;
			repository.SetUpdated(item);
			return Ok();
		}
	}
}