using System;
using System.Windows.Input;

namespace Notifications.Wpf.Command
{
    public class LamdaCommand : ICommand
    {
        private readonly Action<object> _OnExecuteAction;
        private readonly Func<object, bool> _CanExecuteFunc;

        public bool CanExecute(object parameter) { return _CanExecuteFunc?.Invoke(parameter) ?? true; }

        public void Execute(object parameter)
        {
            _OnExecuteAction?.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public LamdaCommand(Action<object> OnExecuteAction, Func<object, bool> CanExecuteFunc = null)
        {
            _OnExecuteAction = OnExecuteAction;
            _CanExecuteFunc = CanExecuteFunc;
        }
    }
}
