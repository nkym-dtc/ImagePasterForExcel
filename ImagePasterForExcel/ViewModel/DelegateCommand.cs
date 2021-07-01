using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImagePasterForExcel.ViewModel
{
    class DelegateCommand : ICommand
    {
        public DelegateCommand(Action<Object> act)
        {
            this._action = act;
        }
        private Action<object> _action;

        public bool CanExecute(object parameter) { return true; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter) { _action(parameter); }
    }
}
