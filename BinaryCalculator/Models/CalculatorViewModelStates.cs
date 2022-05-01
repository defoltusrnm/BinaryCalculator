namespace BinaryCalculator.Application.Models
{
    public enum CalculatorViewModelStates 
    { 
        UserInput, 
        FirstOperandInputed, 
        OperatorInputed, 
        SecondOperandInputed, 
        Calculated, 
        Exception 
    }
}
