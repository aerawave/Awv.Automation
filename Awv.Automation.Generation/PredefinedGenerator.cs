using Awv.Automation.Generation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Awv.Automation.Generation
{
    public class PredefinedGenerator<TGenerationType> : IGenerator<TGenerationType>
    {
        public IEnumerable<TGenerationType> Values { get; set; }
        public PredefinedGenerator(IEnumerable<TGenerationType> values)
        {
            Values = values;
        }

        public virtual PredefinedGenerator<TGenerationType> Filtered(Func<TGenerationType, bool> predicate)
            => new PredefinedGenerator<TGenerationType>(Values.Where(predicate));

        public TGenerationType Generate(IRNG random) => Values.Count() == 0 ? default(TGenerationType) : Values.ElementAt(random.Next(Values.Count()));
    }
}
