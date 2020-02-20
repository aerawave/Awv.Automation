using Awv.Automation.Lexica.Compositional.Lexigrams;
using Awv.Lexica.Compositional;
using Awv.Lexica.Compositional.Lexigrams;
using Awv.Lexica.Compositional.Lexigrams.Interface;
using System;
using System.Linq;
using System.Text;

namespace Awv.Automation.Lexica.Compositional
{
    public class AutomationParser : CompositionParser
    {
        public const char TagStart = '#';
        public const char ConditionalStart = '{';
        public const char ConditionalEnd = '}';
        public const string TagValidChars = "_";

        public AutomationParser(string source) : base(source)
        {
        }

        public override char[] GetStringBreakers()
        {
            return base.GetStringBreakers().Concat(new char[] { TagStart, ConditionalStart, ConditionalEnd }).ToArray();
        }

        public virtual char[] GetTagBreakers()
        {
            return GetStringBreakers().Concat(new char[] { IdStart }).ToArray();
        }

        public virtual bool IsTagValidChar(char c)
        {
            return char.IsLetterOrDigit(c) || TagValidChars.Contains(c);
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
            var tag = ReadTagName();
            var id = (string)null;


            if (Expect(IdStart, true).HasValue)
            {
                id = ReadUntilAny(IdEnd);
                Expect(IdEnd);
            }

            return new TagLexigram(id, tag);
        }

        private string ReadTagName()
        {
            var parsing = true;
            var parsed = new StringBuilder();
            var breakers = GetTagBreakers();
            while (parsing)
            {
                var ch = ReadChar();
                parsing = !EndOfString;
                if (breakers.Contains(ch) || !IsTagValidChar(ch))
                {
                    parsing = false;
                }
                else
                {
                    parsed.Append(ch);
                }
            }
            if (!EndOfString) Back();

            return parsed.ToString();
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

            if (Expect(CodeStart, true).HasValue)
                lexigram.ChanceCode = ReadCode() as CodeLexigram;

            return lexigram;
        }
    }
}
