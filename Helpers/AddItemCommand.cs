using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeTracker
{
    class AddItemCommand : ICommand
    {
        private Action<object> action;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            string[] p = parameter as string[];
            string chargeNumber = p[0];
            string displayName = p[1];

            return !(string.IsNullOrWhiteSpace(chargeNumber) || string.IsNullOrWhiteSpace(displayName));
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }

        public AddItemCommand(Action<object> a)
        {
            action = a;
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
