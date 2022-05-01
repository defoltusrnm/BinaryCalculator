using System.Windows.Input;

namespace BinaryCalculator.Application.Commands.Interfaces
{
    public interface ICommand<TParam> : ICommand
    {
        void Execute(TParam? param);
        bool CanExecute(TParam param);
    }
}
