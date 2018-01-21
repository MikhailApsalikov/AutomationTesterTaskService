using System;

namespace Rokolabs.AutomationTestingTask.Common
{
	public static class GuidConverter
	{
		public static Guid ToGuidWithAccessDenied(this string argument)
		{
			try
			{
				return new Guid(argument);
			}
			catch (Exception e)
			{
				throw new AccessViolationException("Access is denied", e);
			}
		}
	}
}
