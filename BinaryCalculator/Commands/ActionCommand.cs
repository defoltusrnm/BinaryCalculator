using BinaryCalculator.Application.Commands.Base;
using System;

namespace BinaryCalculator.Application.Commands
{
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
