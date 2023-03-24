using System;
using System.Windows.Input;

namespace MTechSystems.Utilerias
{
    public sealed class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute ?? throw new ArgumentNullException("canExecute");
        }

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }
}
