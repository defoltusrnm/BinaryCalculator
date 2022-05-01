using BinaryCalculator.Application.Commands;
using BinaryCalculator.Application.Commands.Interfaces;
using BinaryCalculator.Application.ViewModels.Base;
using System.Windows.Input;

namespace BinaryCalculator.Application.ViewModels
{
    public enum CalcStates { Empty, FirstOperand, WailtSecondOperand, CanCalculate }

    public class CalculatorViewModel : ViewModel
    {
        private CalcStates _state = CalcStates.Empty;
        
        private string _leftOperand = string.Empty;
        private string _rightOperand = string.Empty;
        private char _operator = char.MinValue;
        
        private string _output = string.Empty;
        public string Output { get => _output; set => Set(ref _output, value); }

        public ICommand ResetCalcualtionCommand => new ActionCommand(() =>
        {
            _state = CalcStates.Empty;
            _leftOperand = string.Empty;
            _rightOperand = string.Empty;
            _operator = char.MinValue;
            Output = string.Empty;
        });
        
        public ICommand<string> InsertNumberCommand => new ActionCommand<string>(p =>
        {
            if (_state == CalcStates.WailtSecondOperand || _state == CalcStates.FirstOperand)
            {
                Output = "";
                _state = CalcStates.CanCalculate;
            }
            
            Output += p;
        });

        public ICommand<char> AddOperatorCommand => new ActionCommand<char>(p =>
        {
            _operator = p;

            if (_state == CalcStates.Empty)
            {
                _state = CalcStates.FirstOperand;
                _leftOperand = (string)Output.Clone() ?? "";

                return;
            }

            if (_state == CalcStates.FirstOperand)
            {
                _state = CalcStates.WailtSecondOperand;
                _rightOperand = (string)Output.Clone() ?? "";

                return;
            }

            if (_state == CalcStates.CanCalculate)
            {
                _state = CalcStates.WailtSecondOperand;
                // rigth = output; left = left + rigth
            }
        });
    }
}
