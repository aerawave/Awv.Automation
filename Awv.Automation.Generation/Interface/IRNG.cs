namespace Awv.Automation.Generation.Interface
{
    public interface IRNG
    {
        int Next();
        int Next(int maxValue);
        int Next(int minValue, int maxValue);
        double NextDouble();
    }
}
