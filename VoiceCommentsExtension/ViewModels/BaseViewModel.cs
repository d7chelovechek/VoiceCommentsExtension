using System;
using System.ComponentModel;

namespace VoiceCommentsExtension.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void InitializeCommands()
        { }

        public BaseViewModel()
        {
            InitializeCommands();
        }
    }
}