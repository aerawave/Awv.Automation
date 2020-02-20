using Awv.Automation.Lexica.Compositional.Modifiers;
using Awv.Lexica.Compositional;
using Awv.Lexica.Compositional.Interface;
using Awv.Lexica.Compositional.Lexigrams;
using Awv.Lexica.Compositional.Lexigrams.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using V8.Net;

namespace Awv.Automation.Lexica.Compositional.Lexigrams
{
    public class ConditionalLexigram : Composition, IIdLexigram
    {
        public List<IModifier> Modifiers { get; set; } = new List<IModifier>();
        public string Id { get; set; }
        public CodeLexigram ChanceCode { get; set; }

        public ConditionalLexigram()
        {
            ChanceCode = new CodeLexigram(null, "1");
        }
        public ConditionalLexigram(Composition original)
            : this()
        {
            foreach (var lexigram in original)
                Add(lexigram);
        }

        private bool EvaluateChance(ICompositionEngine engine)
        {
            var value = (InternalHandle)ChanceCode.GetValue(engine);

            if (value.IsBoolean)
                return value.AsBoolean;
            if (value.IsInt32)
                return value.AsInt32 >= 1;
            if (value.IsNumber)
                return engine.Execute($"calculate_chance({value.AsDouble})");

            throw new InvalidCastException($"Code does not evaluate to a boolean, or number: {ChanceCode.Code}");
        }

        public override object GetValue(ICompositionEngine engine)
        {
            if (Id != null)
            {
                var value = engine.GetProperty(Id);
                if (value != null)
                    return value;
            }
            if (EvaluateChance(engine))
                return ApplyModifiers(Build(engine));
            else
                return "";
        }

        private string ApplyModifiers(string input)
        {
            var value = input;
            foreach (var modifier in Modifiers)
                value = modifier.Process(value);
            return value;
        }

        public override string ToString()
        {
            var value = new StringBuilder();

            value.Append($"{{{base.ToString()}}}");
            if (!string.IsNullOrWhiteSpace(Id))
                value.Append($"({Id})");

            foreach (var modifier in Modifiers)
                value.Append($":{modifier.Key}");

            var chanceCode = ChanceCode.Code.Trim().ToLower();
            if (chanceCode != "1" && chanceCode != "true")
                value.Append(ChanceCode.ToString());

            return value.ToString();
        }
    }
}
