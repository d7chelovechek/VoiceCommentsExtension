using System.ComponentModel;

namespace VoiceCommentsExtension.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isInitialized;

        public BaseViewModel()
        {
            InitializeCommands();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void InitializeCommands()
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;
        }
    }
}