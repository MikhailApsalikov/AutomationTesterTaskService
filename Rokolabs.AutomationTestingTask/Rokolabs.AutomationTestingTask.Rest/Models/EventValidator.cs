using System;
using System.Linq;
using Rokolabs.AutomationTestingTask.Entities;

namespace Rokolabs.AutomationTestingTask.Rest.Models
{
	public static class EventValidator
	{
		public static string ValidateCorrect(Event e, bool? isInteraction)
		{
			if (e.Duration < 0)
			{
				return "Duration cannot be negative";
			}
			if (string.IsNullOrWhiteSpace(e.Title))
			{
				return "Title cannot be empty";
			}
			if (e.Title.Length < 12)
			{
				return "Title cannot have less than 12 symbols";
			}
			if (e.Title.ToCharArray().All(Char.IsLower))
			{
				return "Title can contain only lower letters";
			}
			if (string.IsNullOrWhiteSpace(e.Broker))
			{
				return "Broker cannot be empty";
			}
			if (e.Broker.Length > 50)
			{
				return "Broker cannot have more than 50 symbols";
			}
			if (e.Location == null)
			{
				return "Location cannot be null";
			}
			if (string.IsNullOrWhiteSpace(e.Location.City) && string.IsNullOrWhiteSpace(e.Location.Country))
			{
				return "Location cannot be empty";
			}
			if (isInteraction.HasValue)
			{
				if (e.Date < DateTime.Now && !isInteraction.Value)
				{
					return "Event cannot have date in the past. Please, create an Interaction";
				}
				if (e.Date > DateTime.Now && isInteraction.Value)
				{
					return "Interaction cannot have date in the future. Please, create an Event";
				}
			}
			return string.Empty;
		}

		public static string ValidateV2Create(Event e, bool isInteraction)
		{
			if (e.Duration < 1)
			{
				return "Duration cannot be negative";
			}
			if (string.IsNullOrWhiteSpace(e.Title))
			{
				return "Title cannot be empty";
			}
			if (e.Title.Length < 12)
			{
				return "Title cannot have less than 12 symbols";
			}
			if (e.Title.ToCharArray().All(Char.IsLower))
			{
				return "Title can contain only lower letters";
			}
			if (string.IsNullOrWhiteSpace(e.Broker))
			{
				return "Broker cannot be empty";
			}
			if (e.Broker.Length > 50)
			{
				return "Broker cannot have more than 50 symbols";
			}
			if (e.Location == null)
			{
				return "Location cannot be null";
			}
			if (string.IsNullOrWhiteSpace(e.Location.City) && string.IsNullOrWhiteSpace(e.Location.Country))
			{
				return "Location cannot be empty";
			}
			if (e.Date < DateTime.Now && !isInteraction)
			{
				return "Event cannot have date in the past. Please, create an Interaction";
			}
			return string.Empty;
		}

		public static string ValidateV2Update(Event e, bool isInteraction)
		{
			if (e.Duration < 0)
			{
				return "Duration cannot be negative";
			}
			if (string.IsNullOrWhiteSpace(e.Title))
			{
				return "Title cannot be empty";
			}
			if (e.Title.Length < 24)
			{
				return "Title cannot have less than 24 symbols";
			}
			if (e.Title.ToCharArray().All(Char.IsLower))
			{
				return "Title can contain only lower letters";
			}
			if (e.Location == null)
			{
				return "Location cannot be null";
			}
			if (string.IsNullOrWhiteSpace(e.Location.Country))
			{
				return "Location cannot be empty";
			}
			if (e.Date < DateTime.Now && !isInteraction)
			{
				return "Event cannot have date in the past. Please, create an Interaction";
			}
			if (e.Date > DateTime.Now && isInteraction)
			{
				return "Interaction cannot have date in the future. Please, create an Event";
			}
			return string.Empty;
		}

		public static string ValidateV3Create(Event e, bool? isInteraction)
		{
			if (e.Duration < 1)
			{
				return "Duration cannot be negative";
			}
			if (string.IsNullOrWhiteSpace(e.Title))
			{
				return "Title cannot be empty";
			}
			if (e.Title.Length < 12)
			{
				return "Title cannot have less than 12 symbols";
			}
			if (e.Title.ToCharArray().All(Char.IsLower))
			{
				return "Title can contain only lower letters";
			}
			if (string.IsNullOrWhiteSpace(e.Broker))
			{
				return "Broker cannot be empty";
			}
			if (e.Broker.Length > 50)
			{
				return "Broker cannot have more than 50 symbols";
			}
			if (e.Location == null)
			{
				return "Location cannot be null";
			}
			if (string.IsNullOrWhiteSpace(e.Location.City) && string.IsNullOrWhiteSpace(e.Location.Country))
			{
				return "Location cannot be empty";
			}
			if (isInteraction.HasValue)
			{
				if (e.Date < DateTime.Now && !isInteraction.Value)
				{
					return "Event cannot have date in the past. Please, create an Interaction";
				}
				if (e.Date > DateTime.Now && isInteraction.Value)
				{
					return "Interaction cannot have date in the future. Please, create an Event";
				}
			}
			return string.Empty;
		}

		public static string ValidateV3Update(Event e, bool isInteraction)
		{
			if (e.Duration < 0)
			{
				return "Duration cannot be negative";
			}
			if (string.IsNullOrWhiteSpace(e.Title))
			{
				return "Title cannot be empty";
			}
			if (e.Title.Length < 24)
			{
				return "Title cannot have less than 24 symbols";
			}
			if (e.Title.ToCharArray().All(Char.IsLower))
			{
				return "Title can contain only lower letters";
			}
			if (string.IsNullOrWhiteSpace(e.Broker))
			{
				return "Broker cannot be empty";
			}
			if (e.Broker.Length > 50)
			{
				return "Broker cannot have more than 50 symbols";
			}
			if (e.Location == null)
			{
				return "Location cannot be null";
			}
			if (string.IsNullOrWhiteSpace(e.Location.City) && string.IsNullOrWhiteSpace(e.Location.Country))
			{
				return "Location cannot be empty";
			}
			if (e.Date < DateTime.Now && !isInteraction)
			{
				return "Event cannot have date in the past. Please, create an Interaction";
			}
			if (e.Date > DateTime.Now && isInteraction)
			{
				return "Interaction cannot have date in the future. Please, create an Event";
			}
			return string.Empty;
		}
	}
}