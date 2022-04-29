using BinaryCalculator.Converters.Interfaces;

namespace BinaryCalculator.Converters
{
    public class BinaryConverter : IConverter
    {
        public int Mask => 0x1;

        public long ConvertFrom(string value)
        {
            bool isParsed = long.TryParse(value, out long number);
            if (!isParsed)
            {
                throw new ArgumentException($"{value} is not a binary number");
            }

            double result = 0;
            int pow = 0;
            while (number > 0)
            {
                long c = number % 10;
                result += c * Math.Pow(2, pow++);
                number /= 10;
            }

            return (long)result;
        }

        public string ConvertTo(long value)
        {
            int sign = Math.Sign(value);
            value = Math.Abs(value);
            
            var binary = string.Empty;
            while (value > 0)
            {
                binary = (value & Mask) + binary;
                value = value >> Mask;
            }

            return (sign == -1 ? '-' : '\0') + binary;
        }
    }
}
