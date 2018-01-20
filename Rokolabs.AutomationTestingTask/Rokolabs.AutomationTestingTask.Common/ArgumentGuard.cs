using System;

namespace Rokolabs.AutomationTestingTask.Common
{
	public static class ArgumentGuard
	{
		public static void ThrowIfNull<T>(this T argument, string name) where T : class
		{
			if (argument == null)
			{
				throw new ArgumentNullException(name);
			}
		}

		public static void ThrowIfEmpty(this string argument, string name)
		{
			if (string.IsNullOrWhiteSpace(argument))
			{
				throw new ArgumentNullException(name);
			}
		}
	}
}
