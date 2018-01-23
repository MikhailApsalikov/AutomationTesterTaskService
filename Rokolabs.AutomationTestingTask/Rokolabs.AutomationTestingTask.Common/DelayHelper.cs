using System.Threading;

namespace Rokolabs.AutomationTestingTask.Common
{
	public static class DelayHelper
	{
		public static void UnacceptableLongDelay()
		{
			Thread.Sleep(RandomGenerator.Instance.Generate(2300, 3200));
		}

		public static void LongDelay()
		{
			Thread.Sleep(RandomGenerator.Instance.Generate(600, 1000));
		}

		public static void NormalDelay()
		{
			Thread.Sleep(RandomGenerator.Instance.Generate(200, 600));
		}
	}
}
