using System;

namespace Rokolabs.AutomationTestingTask.Entities.Enums
{
	[Flags]
	public enum AddressTypes
	{
		None = 0,
		Home = 1,
		Away = 2,
		Both = Home | Away
	}
}