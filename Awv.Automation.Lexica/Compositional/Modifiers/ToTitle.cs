using System.Text;
using System.Text.RegularExpressions;

namespace Awv.Automation.Lexica.Compositional.Modifiers
{
    public class ToTitle : IModifier
    {
        public string Key { get; set; } = "T";
        public string Process(string value)
        {
            var words = new Regex(@"\w+").Matches(value);
            var builder = new StringBuilder();
            var lastIndex = 0;
            
            for (var i = 0; i < words.Count;i++)
            {
                var word = words[i];
                builder.Append(value.Substring(0, lastIndex));
                builder.Append(word.Value.Substring(0, 1));
                builder.Append(word.Value.Substring(1));
            }

            return builder.ToString();
        }
        public override string ToString() => nameof(ToUpper);
    }
}
