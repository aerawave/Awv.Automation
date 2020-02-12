using Awv.Automation.Generation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Awv.Automation.Lexica
{
    public class PhraseGenerator : PredefinedGenerator<Phrase>
    {
        public PhraseGenerator(IEnumerable<Phrase> values) : base(values)
        {
        }

        public override PredefinedGenerator<Phrase> Filtered(Func<Phrase, bool> predicate)
            => new PhraseGenerator(Values.Where(predicate));

        public PhraseGenerator Tagged(string tag)
            => Filtered(phrase => phrase.IsTagged(tag)) as PhraseGenerator;
    }
}
