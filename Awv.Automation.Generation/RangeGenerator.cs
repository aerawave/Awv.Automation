using Awv.Automation.Generation.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awv.Automation.Generation
{
    public class RangeGenerator : IGenerator<int>
    {
        public int Minimum { get; set; } = 0;
        public int Maximum { get; set; } = 0;

        public RangeGenerator()
        {
            Minimum = 0;
            Maximum = 0;
        }

        public RangeGenerator(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public RangeGenerator SetRange(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
            return this;
        }

        public int Generate(IRNG random)
        {
            if (Maximum < Minimum)
                throw new ArgumentException($"{nameof(Minimum)} or {nameof(Maximum)} value for {nameof(RangeGenerator)} is invalid. {nameof(Maximum)} must be greater than or equal to {nameof(Minimum)}.");

            return random.Next(Minimum, Maximum);
        }
    }
}
