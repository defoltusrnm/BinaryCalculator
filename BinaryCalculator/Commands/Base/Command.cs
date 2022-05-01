using BinaryCalculator.Application.Commands.Interfaces;
using System;
using System.Windows.Input;

namespace BinaryCalculator.Application.Commands.Base
{
    public abstract class Command<TParam> : ICommand<TParam>
    {
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        bool ICommand.CanExecute(object? parameter)
        {
            if (parameter is TParam param)
            {
                return CanExecute(param);
            }

            throw new ArgumentException($"parameter is not {typeof(TParam).Name}");
        }

        public abstract bool CanExecute(TParam param);

        void ICommand.Execute(object? parameter)
        {
            if (parameter is TParam param)
            {
                Execute(param);
            }

            throw new ArgumentException($"parameter is not {typeof(TParam).Name}");
        }

        public abstract void Execute(TParam? param);
    }
}
