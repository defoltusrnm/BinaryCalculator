using BinaryCalculator.Application.Calculators.Interfaces;
using BinaryCalculator.Application.Commands;
using BinaryCalculator.Application.Commands.Interfaces;
using BinaryCalculator.Application.Models;
using BinaryCalculator.Application.ViewModels.Base;
using System;
using System.Windows.Input;

namespace BinaryCalculator.Application.ViewModels
{
    public class CalculatorViewModel : ViewModel
    {
        private CalculatorViewModelStates _state = CalculatorViewModelStates.UserInput;
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
                case CalculatorViewModelStates.SecondOperandInputed:
                    _state = CalculatorViewModelStates.OperatorInputed;
                    _firstOperand = Output.Clone() as string ?? string.Empty;
                    Output = string.Empty;
                    goto case CalculatorViewModelStates.OperatorInputed;
                case CalculatorViewModelStates.FirstOperandInputed:
                    _state = CalculatorViewModelStates.OperatorInputed;
                    Output = string.Empty;
                    goto case CalculatorViewModelStates.OperatorInputed;
                case CalculatorViewModelStates.Calculated:
                    _state = CalculatorViewModelStates.UserInput;
                    Output = string.Empty;
                    goto case CalculatorViewModelStates.UserInput;
                case CalculatorViewModelStates.Exception:
                    Output = string.Empty;
                    _state = CalculatorViewModelStates.UserInput;
                    goto case CalculatorViewModelStates.UserInput;
                case CalculatorViewModelStates.OperatorInputed:
                    Output += parameter;
                    break;
                case CalculatorViewModelStates.UserInput:
                    Output += parameter;
                    break;
            }
        }

        public ICommand<char> InputOperatorCommand => new ActionCommand<char>(OnInputOperatorCommandExecuted, p => !string.IsNullOrEmpty(Output) &&
                                                                                                                   _state != CalculatorViewModelStates.Exception);

        private void OnInputOperatorCommandExecuted(char parameter)
        {
            _operator = parameter;
            switch (_state)
            {
                case CalculatorViewModelStates.UserInput:
                    _firstOperand = Output.Clone() as string ?? string.Empty;
                    _state = CalculatorViewModelStates.FirstOperandInputed;
                    break;
                case CalculatorViewModelStates.OperatorInputed:
                    _secondOperand = Output.Clone() as string ?? string.Empty;
                    _state = CalculatorViewModelStates.SecondOperandInputed;
                    goto case CalculatorViewModelStates.SecondOperandInputed;
                case CalculatorViewModelStates.SecondOperandInputed:
                    _firstOperand = TryCalculate(_firstOperand, Output) ? Output.Clone() as string ?? string.Empty : _firstOperand;
                    _state = CalculatorViewModelStates.FirstOperandInputed;
                    break;
                case CalculatorViewModelStates.Calculated:
                    _firstOperand = Output.Clone() as string ?? string.Empty;
                    _state = CalculatorViewModelStates.FirstOperandInputed;
                    break;
            }
        }

        public ICommand CalculateCommand => new ActionCommand(OnCalculateCommandExecuted, 
                                                              () => _state != CalculatorViewModelStates.UserInput && 
                                                                    _state != CalculatorViewModelStates.FirstOperandInputed &&
                                                                    _state != CalculatorViewModelStates.Exception);

        private void OnCalculateCommandExecuted()
        {
            switch (_state)
            {
                case CalculatorViewModelStates.OperatorInputed:
                    _secondOperand = Output.Clone() as string ?? string.Empty;
                    _state = CalculatorViewModelStates.SecondOperandInputed;
                    goto case CalculatorViewModelStates.SecondOperandInputed;
                case CalculatorViewModelStates.SecondOperandInputed:
                    _state = CalculatorViewModelStates.Calculated;
                    TryCalculate(_firstOperand, _secondOperand);
                    break;
                case CalculatorViewModelStates.Calculated:
                    TryCalculate(Output, _secondOperand);
                    break;
            }
        }

        public ICommand ClearLastInputCommand => new ActionCommand(OnClearLastInputCommandExecuted, () => !string.IsNullOrEmpty(Output) &&
                                                                                                           _state != CalculatorViewModelStates.Exception &&
                                                                                                           _state != CalculatorViewModelStates.FirstOperandInputed);

        private void OnClearLastInputCommandExecuted()
        {
            switch (_state)
            {
                case CalculatorViewModelStates.Calculated:
                    Output = "";
                    _state = CalculatorViewModelStates.UserInput;
                    break;
                default:
                    Output = !string.IsNullOrEmpty(Output) ? Output.Remove(Output.Length - 1) : Output;
                    break;
            }
        }

        public ICommand ResetAllCommand => new ActionCommand(OnResetAllCommandExecuted);

        private void OnResetAllCommandExecuted()
        {
            _state = CalculatorViewModelStates.UserInput;
            Output = string.Empty;
            ResetParameters();
        }

        private bool TryCalculate(string firstValue, string secondValue)
        {
            try
            {
                Output = _calculator.Calculate(firstValue, secondValue, _operator);

                return true;
            }
            catch (Exception e)
            {
                Output = e.Message;
                ResetParameters();
                _state = CalculatorViewModelStates.Exception;

                return false;
            }
        }

        private void ResetParameters()
        {
            _firstOperand = "";
            _secondOperand = "";
            _operator = char.MinValue;
        }
    }
}
