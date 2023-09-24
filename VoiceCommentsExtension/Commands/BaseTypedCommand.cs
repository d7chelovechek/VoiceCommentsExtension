using System;
using System.Windows.Input;

namespace VoiceCommentsExtension.Commands
{
    public abstract class BaseTypedCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public virtual bool CanExecute(T parameter)
        {
            return true;
        }

        public virtual void Execute(T parameter)
        {
            throw new NotImplementedException();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            Execute((T)parameter);
        }
    }
}