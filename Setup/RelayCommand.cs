using System;
using System.Windows.Input;

namespace String_Hashing.Setup
{
    public class RelayCommand : ICommand
    {
        public RelayCommand(Action cmdactone)
        {
            this.cmdactone = cmdactone;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            cmdactone();
        }

        private Action cmdactone;
    }
}
