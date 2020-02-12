using Awv.Automation.Generation.Interface;
using System;

namespace Awv.Automation.Generation
{
    /// <summary>
    /// Random number generator.
    /// </summary>
    public class RNG : IRNG
    {
        public Random Random { get; set; }
        public Sequence Sequence { get; set; } = new Sequence();

        public RNG()
        {
            Random = new Random();
        }

        public RNG(int seed)
        {
            Random = new Random(seed);
        }

        public RNG(Random random)
        {
            Random = random;
        }

        public int Next()
        {
            var val = Random.Next();

            Sequence.Integers.Add(val);

            return val;
        }
        public int Next(int maxValue)
        {
            var val = Random.Next(maxValue); ;

            Sequence.Integers.Add(val);

            return val;
        }
        public int Next(int minValue, int maxValue)
        {
            var val = Random.Next(minValue, maxValue);

            Sequence.Integers.Add(val);

            return val;
        }

        public double NextDouble()
        {
            var val = Random.NextDouble();

            Sequence.Doubles.Add(val);

            return val;
        }
    }
}
