namespace BinaryCalculator.Application.Calculators.Interfaces
{
    public interface ICalculator
    {
        string Calculate(string firstValue, string secondValue, char mathOperator);
    }
}
