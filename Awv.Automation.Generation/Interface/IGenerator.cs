namespace Awv.Automation.Generation.Interface
{
    public interface IGenerator { }
    public interface IGenerator<TGenerationType> : IGenerator
    {
        TGenerationType Generate(IRNG random);
    }
}
