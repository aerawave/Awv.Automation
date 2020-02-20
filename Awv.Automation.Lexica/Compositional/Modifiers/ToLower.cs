namespace Awv.Automation.Lexica.Compositional.Modifiers
{
    public class ToLower : IModifier
    {
        public string Key { get; set; } = "L";
        public string Process(string value) => value.ToLower();
        public override string ToString() => nameof(ToLower);
    }
}
