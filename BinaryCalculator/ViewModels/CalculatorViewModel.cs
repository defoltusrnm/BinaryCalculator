using BinaryCalculator.Application.Commands;
using BinaryCalculator.Application.ViewModels.Base;
using System.Windows.Input;

namespace BinaryCalculator.Application.ViewModels
{
    public class CalculatorViewModel : ViewModel
    {
        public CalculatorViewModel()
        {

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

        public ICommand InsertOperatorCommand => new ActionCommand(OnInsertNumberCommandExecuted,
                                                                   p => !string.IsNullOrEmpty(Result) &&
                                                                        !(Result.Contains('+') || Result.Contains('-')));

        public ICommand DeleteLastInputCommand => new ActionCommand(p => Result = Result.Remove(Result.Length - 1), p => !string.IsNullOrEmpty(Result));

        public ICommand ClearResultCommand => new ActionCommand(p => Result = "", p => !string.IsNullOrEmpty(Result));

        public ICommand CalculateCommand => new ActionCommand(OnCalculateCommandExecuted, p => !string.IsNullOrEmpty(Result));

        private void OnCalculateCommandExecuted(object? parameter)
        {

        }
    }
}
