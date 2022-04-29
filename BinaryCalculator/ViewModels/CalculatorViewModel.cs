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
        private bool _isCalculated = false;

        public CalculatorViewModel(IConverter converter)
        {
            _converter = converter;
        }

        #region Result Prop
        private string _result = string.Empty;
        public string Result { get => _result; set => Set(ref _result, value); }
        #endregion Result Prop

        #region InsertNumber Command
        public ICommand InsertNumberCommand => new ActionCommand(OnInsertNumberCommandExecuted);

        private void OnInsertNumberCommandExecuted(object? parameter)
        {
            if (parameter is string number)
            {
                if (_isCalculated)
                {
                    _isCalculated = false;
                    ClearResultCommand.Execute(null);
                }
                
                Result += number;
            }
        }
        #endregion InsertNumber Command

        #region InsertOperator Command
        public ICommand InsertOperatorCommand => new ActionCommand(OnInsertOperatorCommandExecuted,
                                                                   p => !string.IsNullOrEmpty(Result) &&
                                                                        !(Result.Contains(Plus) || Result.Contains(Minus)) &&
                                                                        !_isCalculated);
        
        private void OnInsertOperatorCommandExecuted(object? parameter)
        {
            OnInsertNumberCommandExecuted(parameter);
            _operatorIndex = Result.Length - 1;
        }
        #endregion InsertOperator Command

        #region DeleteLastInput Command
        public ICommand DeleteLastInputCommand => new ActionCommand(OnDeleteLastInputCommandExecute, p => !string.IsNullOrEmpty(Result));

        private void OnDeleteLastInputCommandExecute(object? parameter)
        {
            if (_isCalculated)
            {
                ClearResultCommand?.Execute(null);
            }
            
            if (_operatorIndex != -1 && Result[^1] == Result[_operatorIndex])
            {
                _operatorIndex--;
            }
            
            Result = string.IsNullOrEmpty(Result) ? Result : Result.Remove(Result.Length - 1);
        }
        #endregion DeleteLastInput Command

        #region ClearResult Command
        public ICommand ClearResultCommand => new ActionCommand(OnClearResultCommandExecuted, p => !string.IsNullOrEmpty(Result));

        private void OnClearResultCommandExecuted(object? parameter)
        {
            Result = "";
            _operatorIndex = -1;
        }
        #endregion ClearResult Command

        #region Calculate Command
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
            
            _isCalculated = true;
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
        #endregion Calculate Command
    }
}
