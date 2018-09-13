using System.Windows.Input;
using System;

namespace Services
{
    public class RelayCommand : ICommand
    {
        #region Fields

        private Action<object> _Execute;
        private Func<object, bool> _CanExecute;
        
        #endregion Fields

        #region Properties

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        #endregion Properties

        #region Constructor

        public RelayCommand(Action<object> Execute, Func<object, bool> canExecute = null)
        {
            _Execute = Execute;
            _CanExecute = canExecute;
        }

        #endregion Constructor

        #region Methods

        public bool CanExecute(object parameter) => _CanExecute is null || _CanExecute(parameter);
        public void Execute(object parameter) => _Execute(parameter);

        #endregion Methods
    }
}