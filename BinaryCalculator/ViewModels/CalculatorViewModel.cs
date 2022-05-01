using BinaryCalculator.Application.Calculators.Interfaces;
using BinaryCalculator.Application.Commands;
using BinaryCalculator.Application.Commands.Interfaces;
using BinaryCalculator.Application.ViewModels.Base;
using System.Windows.Input;

namespace BinaryCalculator.Application.ViewModels
{
    public enum States { UserInput, FirstOperandInputed, OperatorInputed, SecondOperandInputed, Calculated }
    
    public class CalculatorViewModel : ViewModel
    {
        private States _state = States.UserInput;
        private char _operator = char.MinValue;
        private string _firstOperand = string.Empty;
        private string _secondOperand = string.Empty;
        private readonly ICalculator _calculator;
        
        public CalculatorViewModel(ICalculator calculator)
        {
            _calculator = calculator;
        }

        private string _output = string.Empty;
        public string Output { get => _output; set => Set(ref _output, value); }

        public ICommand<string> InputNumberCommand => new ActionCommand<string>(OnInputNumberCommandExecuted);

        private void OnInputNumberCommandExecuted(string? parameter)
        {
            switch (_state)
            {
                case States.SecondOperandInputed:
                    _state = States.OperatorInputed;
                    _firstOperand = Output.Clone() as string ?? string.Empty;
                    Output = "";
                    goto case States.OperatorInputed;
                case States.FirstOperandInputed:
                    _state = States.OperatorInputed;
                    Output = "";
                    goto case States.OperatorInputed;
                case States.Calculated:
                    _state = States.UserInput;
                    Output = "";
                    goto case States.UserInput;
                case States.OperatorInputed:
                    Output += parameter;
                    break;
                case States.UserInput:
                    Output += parameter;
                    break;
            }
        }

        public ICommand<char> InputOperatorCommand => new ActionCommand<char>(OnInputOperatorCommandExecuted, p => !string.IsNullOrEmpty(Output));

        private void OnInputOperatorCommandExecuted(char parameter)
        {
            _operator = parameter;
            switch (_state)
            {
                case States.UserInput:
                    _firstOperand = Output.Clone() as string ?? string.Empty;
                    _state = States.FirstOperandInputed;
                    break;
                case States.OperatorInputed:
                    _secondOperand = Output.Clone() as string ?? string.Empty;
                    _state = States.SecondOperandInputed;
                    goto case States.SecondOperandInputed;
                case States.SecondOperandInputed:
                    _firstOperand = _calculator.Calculate(_firstOperand, Output, _operator);
                    Output = _firstOperand.Clone() as string ?? string.Empty;
                    _state = States.FirstOperandInputed;
                    break;
                case States.Calculated:
                    _secondOperand = Output.Clone() as string ?? string.Empty;
                    _state = States.SecondOperandInputed;
                    break;
            }
        }

        public ICommand CalculateCommand => new ActionCommand(OnCalculateCommandExecuted);

        private void OnCalculateCommandExecuted()
        {
            if (_state == States.OperatorInputed || _state == States.SecondOperandInputed || _state == States.Calculated)
            {
                _secondOperand = Output.Clone() as string ?? string.Empty;
                Output = _calculator.Calculate(_firstOperand, _secondOperand, _operator);
                
                _state = States.Calculated;
            }
        }
    }
}
