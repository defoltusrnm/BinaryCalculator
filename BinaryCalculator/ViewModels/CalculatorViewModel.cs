using BinaryCalculator.Application.ViewModels.Base;

namespace BinaryCalculator.Application.ViewModels
{
    public class CalculatorViewModel : ViewModel
    {
        private string _output = "1213123213";
        public string Output { get => _output; set => Set(ref _output, value); }
    }
}
