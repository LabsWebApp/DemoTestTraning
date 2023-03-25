using System.Windows.Input;

namespace ViewModelCommandBases.Commands
{
    public class CommonCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public CommonCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter = null) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter)
        {
            if (CanExecute()) _execute?.Invoke();
            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
