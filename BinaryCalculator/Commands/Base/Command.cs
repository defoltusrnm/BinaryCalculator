using BinaryCalculator.Application.Commands.Interfaces;
using System;
using System.Windows.Input;

namespace BinaryCalculator.Application.Commands.Base
{
    public abstract class Command : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public abstract bool CanExecute(object? parameter);

        public abstract void Execute(object? parameter);
    }

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

            return CanExecute(default);
        }

        public abstract bool CanExecute(TParam? param);

        void ICommand.Execute(object? parameter)
        {
            if (parameter is TParam param)
            {
                Execute(param);
                
                return;
            }

            Execute(default);
        }

        public abstract void Execute(TParam? param);
    }
}
