﻿using Awv.Lexica.Compositional;
using Awv.Lexica.Compositional.Interface;
using Awv.Lexica.Compositional.Lexigrams;
using Awv.Lexica.Compositional.Lexigrams.Interface;
using System;
using V8.Net;

namespace Awv.Automation.Lexica.Compositional.Lexigrams
{
    public class ConditionalLexigram : Composition, IIdLexigram
    {
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
            if (EvaluateChance(engine))
                return Build(engine);
            else
                return "";
        }

        public override string ToString()
            => $"{{{base.ToString()}}}{(!string.IsNullOrWhiteSpace(Id) ? $"({Id})" : "")}{(ChanceCode.Code.Trim() == "1" || ChanceCode.Code.Trim().ToLower() == "true" ? "" : ChanceCode.ToString())}";
    }
}