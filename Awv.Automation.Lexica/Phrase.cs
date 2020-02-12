using System;
using System.Collections.Generic;
using System.Linq;

namespace Awv.Automation.Lexica
{
    public class Phrase : ICloneable
    {
        public string Value { get; set; }
        private List<string> Tags { get; set; } = new List<string>();
        public string[] AllTags => Tags.ToArray();
        
        public Phrase(string value)
        {
            Value = value;
            Tag(Value);
        }

        public Phrase(string value, params string[] tags)
            : this(value)
        {
            foreach (var tag in tags)
                Tag(tag);
        }

        public bool IsTagged(string tag) => Tags.Contains(ProcessTag(tag));
        public string GetFirstTag(params string[] tags)
            => tags.FirstOrDefault(tag => IsTagged(tag));
        
        public void Tag(string tag)
        {
            tag = ProcessTag(tag);

            if (!IsTagged(tag))
                Tags.Add(tag);
        }

        public void Untag(string tag)
        {
            tag = ProcessTag(tag);

            if (IsTagged(tag))
                Tags.Remove(tag);
        }

        private string ProcessTag(string tag)
            => tag.Replace(" ", "").ToLower();

        public object Clone()
        {
            return new Phrase(Value)
            {
                Tags = Tags.ToList()
            };
        }

        public override string ToString() => Value;

        public static implicit operator Phrase(string phrase) => new Phrase(phrase);
    }
}
