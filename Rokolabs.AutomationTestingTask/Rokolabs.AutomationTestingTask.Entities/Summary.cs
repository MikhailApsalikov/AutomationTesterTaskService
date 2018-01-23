namespace Rokolabs.AutomationTestingTask.Entities
{
	public class Summary
	{
		public int Count { get; set; }
		public int InteractionCount { get; set; }
		public int EventCount { get; set; }
		public double AverageDuration { get; set; }
		public string MostPopularBroker { get; set; }
	}
}
