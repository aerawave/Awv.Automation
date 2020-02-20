using Awv.Automation.Lexica.Compositional.Lexigrams;
using Awv.Automation.Lexica.Compositional.Modifiers;
using Awv.Lexica.Compositional;
using Awv.Lexica.Compositional.Lexigrams;
using Awv.Lexica.Compositional.Lexigrams.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Awv.Automation.Lexica.Compositional
{
    public class AutomationParser : CompositionParser
    {
        public const char TagStart = '#';
        public const char ConditionalStart = '{';
        public const char ConditionalEnd = '}';
        public const char ModifierStart = ':';
        public const string TagValidChars = "_";
        public const string ModifierValidChars = "_";

        public Dictionary<string, IModifier> Modifiers { get; set; } = new Dictionary<string, IModifier>();

        public AutomationParser(string source) : base(source)
        {
            AllowModifier<ToLower>();
            AllowModifier<ToUpper>();
            AllowModifier<ToTitle>();
        }

        public void AllowModifier<TModifier>() where TModifier : IModifier => AllowModifier(Activator.CreateInstance<TModifier>());
        public void AllowModifier<TModifier>(string key) where TModifier : IModifier => AllowModifier(key, Activator.CreateInstance<TModifier>());
        public void AllowModifier(IModifier modifier) => AllowModifier(modifier.Key, modifier);
        public void AllowModifier(string key, IModifier modifier)
        {
            if (Modifiers.ContainsKey(key)) Modifiers[key] = modifier;
            else Modifiers.Add(key, modifier);
        }

        public override char[] GetStringBreakers()
        {
            return base.GetStringBreakers().Concat(new char[] { TagStart, ConditionalStart, ConditionalEnd }).ToArray();
        }

        public virtual char[] GetTagBreakers()
        {
            return GetStringBreakers().Concat(new char[] { IdStart }).ToArray();
        }

        /// <summary>
        /// Reads the next <see cref="ILexigram"/>. This could be a <see cref="Lexigram"/> or a <see cref="CodeLexigram"/>.
        /// </summary>
        /// <returns>The next <see cref="ILexigram"/></returns>
        public override ILexigram ReadNext()
        {
            ILexigram output;
            if (Expect(TagStart, true).HasValue)
            {
                output = ReadTag();
            } else if (Expect(ConditionalStart, true).HasValue)
            {
                output = ReadConditional();
            }
            else
            {
                output = base.ReadNext();
            }
            return output;
        }

        private TagLexigram ReadTag()
        {
            var tagName = ReadTagName();
            var id = (string)null;


            if (Expect(IdStart, true).HasValue)
            {
                id = ReadUntilAny(IdEnd);
                Expect(IdEnd);
            }

            var tag = new TagLexigram(id, tagName);

            while (Expect(ModifierStart, true).HasValue)
                tag.Modifiers.Add(ReadModifier());


            return tag;
        }

        private string ReadTagName()
        {
            var breakers = GetTagBreakers();
            var tagName = ReadWhile(ch => !EndOfString && !breakers.Contains(ch) && (TagValidChars.Contains(ch) || char.IsLetterOrDigit(ch)));
            return tagName;
        }

        public ConditionalLexigram ReadConditional()
        {
            var parsing = true;
            var parsed = new StringBuilder();
            var depth = 0;
            while (parsing)
            {
                var ch = ReadChar();
                parsing = !EndOfString;
                switch (ch)
                {
                    case EscapeChar:
                        ch = ReadChar();
                        parsed.Append(ch);
                        break;
                    default:
                        parsed.Append(ch);
                        break;
                    case ConditionalStart:
                        depth++;
                        parsed.Append(ch);
                        break;
                    case ConditionalEnd:
                        if (depth-- == 0)
                        {
                            parsing = false;
                        } else
                        {
                            parsed.Append(ch);
                        }
                        break;
                }
            }
            if (!EndOfString) Back();
            Expect(ConditionalEnd);

            var oldcomp = new AutomationParser(parsed.ToString()).Transpile();
            var lexigram = new ConditionalLexigram(oldcomp);

            if (Expect(IdStart, true).HasValue)
            {
                lexigram.Id = ReadUntilAny(IdEnd);
                Expect(IdEnd);
            }

            while (Expect(ModifierStart, true).HasValue)
                lexigram.Modifiers.Add(ReadModifier());

            if (Expect(CodeStart, true).HasValue)
                lexigram.ChanceCode = ReadCode() as CodeLexigram;

            return lexigram;
        }

        public IModifier ReadModifier()
        {
            var modifierKey = ReadWhile(ch => char.IsLetterOrDigit(ch) || ModifierValidChars.Contains(ch));
            if (Modifiers.ContainsKey(modifierKey))
            {
                return Modifiers[modifierKey];
            }
            return null;
        }




    }
}
