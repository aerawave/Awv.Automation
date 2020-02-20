using Awv.Automation.Lexica.Compositional.Modifiers;
using Awv.Lexica.Compositional.Interface;
using Awv.Lexica.Compositional.Lexigrams;
using System.Collections.Generic;
using System.Text;

namespace Awv.Automation.Lexica.Compositional.Lexigrams
{
    public class TagLexigram : CodeLexigram, IHasModifiers
    {
        public List<IModifier> Modifiers { get; set; } = new List<IModifier>();
        public override string Id { get => base.Id?.Trim().Length == 0 ? Tag : base.Id; set => base.Id = value; }
        public override string Code
        {
            get => $"tag(\"{Tag}\")";
            set { }
        }
        public string Tag { get; set; }

        public TagLexigram(string id, string tag)
            : base(id, null)
        {
            Tag = tag;
        }

        public override object GetValue(ICompositionEngine engine)
        {
            return ApplyModifiers(base.GetValue(engine).ToString());
        }

        private string ApplyModifiers(string input)
        {
            var value = input;
            foreach (var modifier in Modifiers)
                value = modifier.Process(value);
            return value;
        }


        public static explicit operator TagLexigram(string tag) => new TagLexigram(null, tag);

        public override string ToString()
        {
            var value = new StringBuilder();

            var vv = $"#{Tag}{(Id != null ? (Id == Tag ? "()" : $"({Id})") : "")}";

            value.Append($"#{Tag}");

            if (!string.IsNullOrWhiteSpace(Id))
            {
                if (Id == Tag)
                    value.Append("()");
                else
                    value.Append($"({Id})");
            }

            foreach (var modifier in Modifiers)
                value.Append($":{modifier.Key}");

            return value.ToString();
        }

        public IEnumerable<IModifier> GetModifiers() => Modifiers;
    }
}
