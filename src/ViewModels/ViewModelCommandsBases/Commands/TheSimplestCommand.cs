using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModelCommandsBases.Commands
{
    public class TheSimplestCommand : ICommand
    {
        private readonly Action _execute;

        public TheSimplestCommand(Action execute) => _execute = execute;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => _execute.Invoke();

        public event EventHandler? CanExecuteChanged;
    }
}
