namespace Awv.Automation.Lexica.Compositional.Modifiers
{
    public class ToUpper : IModifier
    {
        public string Key { get; set; } = "U";
        public string Process(string value) => value.ToUpper();
        public override string ToString() => nameof(ToUpper);
    }
}
