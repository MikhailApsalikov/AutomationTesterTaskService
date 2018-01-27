using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Rokolabs.AutomationTestingTask.Common;
using Rokolabs.AutomationTestingTask.Entities;
using Rokolabs.AutomationTestingTask.Entities.Enums;
using Rokolabs.AutomationTestingTask.Entities.Filters;
using Rokolabs.AutomationTestingTask.Rest.Models;

namespace Rokolabs.AutomationTestingTask.Rest.Controllers.v4
{
	[Route("v4/Report")]
	public class ReportV4Controller : ApiController
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
			List<Event> events = repository.GetByFilter(filter);
			HttpContent fileResult;
			switch (filter.FileFormat)
			{
				case FileFormat.Csv:
					fileResult = SerializeCsv(events);
					break;
				case FileFormat.Xml:
					fileResult = SerializeXml(events);
					break;
				case FileFormat.Json:
					fileResult = SerializeJson(events);
					break;
				default:
					return InternalServerError(new ArgumentException("FileFormat"));
			}
			HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
			httpResponseMessage.Content = fileResult;
			httpResponseMessage.Content.Headers.ContentDisposition =
				new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
				{
					FileName = $"report.{filter.FileFormat.ToString().ToLowerInvariant()}"
				};
			httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

			return ResponseMessage(httpResponseMessage);
		}

		private static HttpContent SerializeCsv(List<Event> events)
		{
			List<string> strings = new List<string>();
			strings.Add("\"ID\", Date, Title, Interaction type, Meeting type, Country, City, Duration, Sectors, Address Type, Created, Updated");
			strings.AddRange(events.Select(e => string.Join(", ", new[]
			 {
				e.EventId.ToString(),
				e.Date.ToString("MM/dd/yyyy"),
				e.Title.ToString(),
				e.InteractionType.ToString(),
				string.Join(" ", e.MeetingTypes).ToString(),
				e?.Location?.Country.ToString(),
				e?.Location?.City.ToString(),
				e.Duration.ToString(),
				string.Join(" ", e.Sectors).ToString(),
				e.AddressType.ToString(),
				e?.Created.ToString("MM/dd/yyyy"),
				e?.Updated?.ToString("MM/dd/yyyy"),
			})));
			return new StringContent(string.Join(Environment.NewLine, strings));
		}

		private static HttpContent SerializeXml(List<Event> events)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(List<Event>));
			using (MemoryStream ms = new MemoryStream())
			{
				serializer.Serialize(ms, events);
				return new StringContent(Encoding.ASCII.GetString(ms.ToArray()));
			}
		}

		private static HttpContent SerializeJson(List<Event> events)
		{
			return new StringContent(JsonConvert.SerializeObject(events));
		}
	}
}