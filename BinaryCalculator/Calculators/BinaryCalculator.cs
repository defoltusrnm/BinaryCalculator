using BinaryCalculator.Application.Calculators.Interfaces;
using System;

namespace BinaryCalculator.Application.Calculators
{
    public class BinaryCalculator : ICalculator
    {
        private const char Plus = '+';
        private const char Minus = '-';
        
        public string Calculate(string firstValue, string secondValue, char mathOperator)
        {
            int firstNumber = Convert.ToInt32(firstValue, 2);
            int secondNumber = Convert.ToInt32(secondValue, 2);

            int result = mathOperator switch
            {
                Plus => firstNumber + secondNumber,
                Minus => firstNumber - secondNumber,
                _ => -1
            };

            if (result < 0)
            {
                return "Error: negative numbers are not supported";
            }

            return Convert.ToString(result, 2);
        }
    }
}
