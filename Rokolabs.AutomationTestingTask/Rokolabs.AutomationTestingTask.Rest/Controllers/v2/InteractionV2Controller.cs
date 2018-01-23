﻿using System;
using System.Linq;
using System.Web.Http;
using Rokolabs.AutomationTestingTask.Common;
using Rokolabs.AutomationTestingTask.Entities;
using Rokolabs.AutomationTestingTask.Repositories;
using Rokolabs.AutomationTestingTask.Rest.Models;

namespace Rokolabs.AutomationTestingTask.Rest.Controllers.v2
{
	[Route("v2/Interaction")]
	public class InteractionV2Controller : ApiController
	{
		[HttpDelete]
		public IHttpActionResult Delete(int id, string sessionId)
		{
			var repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());
			var item = repository.GetById(id);
			if (item == null || !repository.IsInteraction(item))
			{
				return InternalServerError();
			}
			return Ok();
		}

		[HttpGet]
		public IHttpActionResult Get(int id, string sessionId)
		{
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
			var repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());
			var items = repository.GetAll().Where(e => repository.IsInteraction(e));
			return Ok(items);
		}

		[HttpPost]
		public IHttpActionResult Post(string sessionId, [FromBody] Event value)
		{
			var repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());
			var validationResult = EventValidator.ValidateV2Create(value, true);
			if (!string.IsNullOrWhiteSpace(validationResult))
			{
				return InternalServerError(new ArgumentException(validationResult));
			}
			var result = repository.Create(value);
			return Ok(result);
		}

		[HttpPut]
		public IHttpActionResult Put(int id, string sessionId, [FromBody] Event value)
		{
			var repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());
			var validationResult = EventValidator.ValidateV2Update(value, true);
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