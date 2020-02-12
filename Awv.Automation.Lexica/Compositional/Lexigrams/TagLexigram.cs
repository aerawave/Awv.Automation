using Awv.Lexica.Compositional.Lexigrams;

namespace Awv.Automation.Lexica.Compositional.Lexigrams
{
    public class TagLexigram : CodeLexigram
    {
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


        public static explicit operator TagLexigram(string tag) => new TagLexigram(null, tag);

        public override string ToString() =>
            $"#{Tag}{(Id != null ? (Id == Tag ? "()" : $"({Id})") : "")}";
    }
}
