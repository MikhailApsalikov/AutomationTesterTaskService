using System;

namespace Rokolabs.AutomationTestingTask.Common
{
    public class RandomGenerator
    {
	    public static RandomGenerator Instance { get; } = new RandomGenerator();
	    private Random Random { get; }

	    private RandomGenerator()
	    {
			Random = new Random((int)DateTime.Now.Ticks);

		}

	    public long Generate(long min, long max)
	    {
		    long result = Random.Next((int)(min >> 32), (int)(max >> 32));
		    result = result << 32;
		    result = result | (long)Random.Next((int)min, (int)max);
		    return result;
		}
    }
}
