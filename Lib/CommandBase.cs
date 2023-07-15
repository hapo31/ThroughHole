using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CamPreview.Lib
{
    public class Command<T> : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Action<T> action;
        private readonly Func<bool> canExecute;

        public Command(Action<T> action, Func<bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute?.Invoke() ?? false;
        }

        public void Execute(object? parameter)
        {
            action?.Invoke(obj: (T)parameter);
        }
    }
}
