using System;
using System.Windows.Input;


namespace MTechSystems.Utilerias
{
    public sealed class RelayCommandParameter : ICommand
    {
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommandParameter(Action<object> execute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
        }

        public RelayCommandParameter(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute ?? throw new ArgumentNullException("canExecute");
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
