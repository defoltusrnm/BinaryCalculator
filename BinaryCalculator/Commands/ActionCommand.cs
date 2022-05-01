using BinaryCalculator.Application.Commands.Base;
using System;

namespace BinaryCalculator.Application.Commands
{
    public class ActionCommand : Command
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public ActionCommand(Action execute, Func<bool>? canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public ActionCommand(Action execute)
        {
            _execute = execute;
        }

        public override bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public override void Execute(object? parameter) => _execute?.Invoke();
    }

    public class ActionCommand<TParam> : Command<TParam>
    {
        private readonly Action<TParam?> _execute;
        private readonly Predicate<TParam?>? _canExecute;

        public ActionCommand(Action<TParam?> execute, Predicate<TParam?> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public ActionCommand(Action<TParam?> execute)
        {
            _execute = execute;
        }

        public override bool CanExecute(TParam? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public override void Execute(TParam? parameter) => _execute?.Invoke(parameter);
    }
}
