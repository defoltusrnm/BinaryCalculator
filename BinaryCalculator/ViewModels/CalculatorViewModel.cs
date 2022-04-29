using BinaryCalculator.Application.Commands;
using BinaryCalculator.Application.ViewModels.Base;
using BinaryCalculator.Converters.Interfaces;
using System;
using System.Windows.Input;

namespace BinaryCalculator.Application.ViewModels
{
    public class CalculatorViewModel : ViewModel
    {
        private const char Plus = '+';
        private const char Minus = '-';

        private readonly IConverter _converter;
        private int _operatorIndex = -1;

        public CalculatorViewModel(IConverter converter)
        {
            _converter = converter;
        }

        private string _result = string.Empty;
        public string Result { get => _result; set => Set(ref _result, value); }

        public ICommand InsertNumberCommand => new ActionCommand(OnInsertNumberCommandExecuted);

        private void OnInsertNumberCommandExecuted(object? parameter)
        {
            if (parameter is string number)
            {
                Result += number;
            }
        }

        public ICommand InsertOperatorCommand => new ActionCommand(OnInsertOperatorCommandExecuted,
                                                                   p => !string.IsNullOrEmpty(Result) &&
                                                                        !(Result.Contains(Plus) || Result.Contains(Minus)));

        private void OnInsertOperatorCommandExecuted(object? parameter)
        {
            OnInsertNumberCommandExecuted(parameter);
            _operatorIndex = Result.Length - 1;
        }

        public ICommand DeleteLastInputCommand => new ActionCommand(p => Result = Result.Remove(Result.Length - 1), p => !string.IsNullOrEmpty(Result));

        public ICommand ClearResultCommand => new ActionCommand(p => Result = "", p => !string.IsNullOrEmpty(Result));

        public ICommand CalculateCommand => new ActionCommand(OnCalculateCommandExecuted,
                                                              CanCalculateCommandExecute);

        private void OnCalculateCommandExecuted(object? parameter)
        {
            var operands = Result.Split(Plus, Minus);
            var leftOperand = _converter.ConvertFrom(operands[0]);
            var rightOperand = _converter.ConvertFrom(operands[1]);

            Result = _converter.ConvertTo(Result[_operatorIndex] switch
            {
                Plus => leftOperand + rightOperand,
                Minus => leftOperand - rightOperand,
                _ => throw new ArgumentException($"Invalid operator char {Result[_operatorIndex]}")
            });
        }

        private bool CanCalculateCommandExecute(object? parameter)
        {
            var isEmpty = string.IsNullOrEmpty(Result);
            if (isEmpty)
            {
                return false;
            }

            var isHaveOperation = Result.Contains(Plus) || Result.Contains(Minus);
            var isHaveRightOperand = !(Result[^1] == Plus || Result[^1] == Minus);

            return isHaveOperation && isHaveRightOperand;
        }
    }
}
