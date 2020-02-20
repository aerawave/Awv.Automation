namespace Awv.Automation.Lexica.Compositional.Modifiers
{
    public class ToUpper : IModifier
    {
        public string Process(string value) => value.ToUpper();
    }
}
