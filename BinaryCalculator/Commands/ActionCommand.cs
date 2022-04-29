using BinaryCalculator.Application.Commands.Base;
using System;

namespace BinaryCalculator.Application.Commands
{
    public class ActionCommand : Command
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public ActionCommand(Action<object?> execute, Predicate<object?> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public ActionCommand(Action<object?> execute)
        {
            _execute = execute;
        }

        public override bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public override void Execute(object? parameter) => _execute?.Invoke(parameter);
    }
}
