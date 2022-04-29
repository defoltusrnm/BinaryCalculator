namespace BinaryCalculator.Converters.Interfaces
{
    public interface IConverter
    {
        public int Mask { get; }
        string ConvertTo(long value);
        long ConvertFrom(string value);
    }
}
