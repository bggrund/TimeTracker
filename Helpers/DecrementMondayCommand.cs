using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeTracker
{
    class DecrementMondayCommand : ICommand
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
            if(parameter == null)
            {
                return false;
            }

            ProjectTimeDataViewModel item = parameter as ProjectTimeDataViewModel;
            
            return item.MondayTime > 0;
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }

        public DecrementMondayCommand(Action<object> a)
        {
            action = a;
        }
    }
}
