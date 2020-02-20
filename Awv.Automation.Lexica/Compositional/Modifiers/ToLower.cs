namespace Awv.Automation.Lexica.Compositional.Modifiers
{
    public class ToLower : IModifier
    {
        public string Process(string value) => value.ToLower();
    }
}
