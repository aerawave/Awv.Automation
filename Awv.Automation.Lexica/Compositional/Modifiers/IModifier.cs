namespace Awv.Automation.Lexica.Compositional.Modifiers
{
    public interface IModifier
    {
        string Key { get; }
        string Process(string value);
    }
}
