using System;
using Rokolabs.AutomationTestingTask.Entities;

namespace Rokolabs.AutomationTestingTask.Rest.Models
{
	public static class EventValidator
	{
		public static string ValidateV4(Event e, bool isInteraction)
		{
			if (e.Duration < 0)
			{
				return "Duration cannot be negative";
			}
			if (string.IsNullOrWhiteSpace(e.Title))
			{
				return "Title cannot be empty";
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