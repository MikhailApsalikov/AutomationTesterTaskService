using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
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

		[HttpPost]
		public IHttpActionResult Post(string sessionId, FileFormat fileFormat)
		{
			if (HttpContext.Current.Request.Files.AllKeys.Any())
			{
				var repository = EventRepositoryCache.Instance.Get(sessionId.ToGuidWithAccessDenied());

				var httpPostedFile = HttpContext.Current.Request.Files["Data"];
				if (httpPostedFile != null)
				{
					string result = StreamToString(httpPostedFile.InputStream);
					List<Event> events;
					try
					{
						switch (fileFormat)
						{
							case FileFormat.Csv:
								events = DeserializeCsv(result);
								break;
							case FileFormat.Xml:
								events = DeserializeXml(result);
								break;
							case FileFormat.Json:
								events = DeserializeJson(result);
								break;
							default:
								return InternalServerError(new ArgumentException("FileFormat"));
						}
					}
					catch (Exception e)
					{
						return InternalServerError(new IOException("Parsing error", e));
					}
					foreach (Event e in events)
					{
						var validationResult = EventValidator.ValidateCorrect(e, null);
						if (!string.IsNullOrWhiteSpace(validationResult))
						{
							return InternalServerError(new ArgumentException(validationResult));
						}
						repository.Create(e);
					}

					return Ok();
				}

				return BadRequest("File is not provided");
			}

			return BadRequest("File is not provided");
		}

		[NonAction]
		private List<Event> DeserializeCsv(string result)
		{
			List<List<string>> rows = result
				.Split(new[]
				{
					Environment.NewLine
				}, StringSplitOptions.RemoveEmptyEntries)
				.Select(e => e
					.Split(',')
					.Select(s => s.Trim())
					.ToList()
				).ToList();
			foreach (List<string> row in rows)
			{
				foreach (string cell in row)
				{
					if (string.IsNullOrWhiteSpace(cell))
					{
						throw new ArgumentNullException("All cells must be filled");
					}
				}
			}
			return rows.Select(e => new Event
				{
					Date = DateTime.Parse(e[0], CultureInfo.GetCultureInfo("en-US")),
					Title = e[1],
					Broker = e[2],
					InteractionType = EnumConverter.ToInteractionType(e[3]),
					MeetingTypes = e[4].Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries)
						.Select(EnumConverter.ToMeetingType).ToList(),
					Location = new Location()
					{
						Country = e[5],
						City = e[6]
					},
					Duration = int.Parse(e[7]),
					Sectors = e[8].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
						.Select(EnumConverter.ToSectors).ToList(),
				AddressType = EnumConverter.ToAddressType(e[9]),
				}).ToList();
		}

		[NonAction]
		private List<Event> DeserializeXml(string result)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(List<Event>));
			using (TextReader reader = new StringReader(result))
			{
				return (List<Event>)serializer.Deserialize(reader);
			}
		}

		[NonAction]
		private List<Event> DeserializeJson(string result)
		{
			return JsonConvert.DeserializeObject<List<Event>>(result);
		}

		[NonAction]
		private static HttpContent SerializeCsv(List<Event> events)
		{
			List<string> strings = new List<string>();
			strings.Add("\"ID\", Date, Title, Broker, Interaction type, Meeting type, Country, City, Duration, Sectors, Address Type, Created, Updated");
			strings.AddRange(events.Select(e => string.Join(", ", new[]
			 {
				e.EventId.ToString(),
				e.Date.ToString("MM/dd/yyyy"),
				e.Title.ToString(),
				e.Broker,
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

		[NonAction]
		private static HttpContent SerializeXml(List<Event> events)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(List<Event>));
			using (MemoryStream ms = new MemoryStream())
			{
				serializer.Serialize(ms, events);
				return new StringContent(Encoding.ASCII.GetString(ms.ToArray()));
			}
		}

		[NonAction]
		private static HttpContent SerializeJson(List<Event> events)
		{
			return new StringContent(JsonConvert.SerializeObject(events));
		}

		[NonAction]
		private string StreamToString(Stream stream)
		{
			stream.Position = 0;
			using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
			{
				return reader.ReadToEnd();
			}
		}
	}
}