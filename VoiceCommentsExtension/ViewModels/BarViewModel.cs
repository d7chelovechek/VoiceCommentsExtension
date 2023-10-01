using System.Windows.Input;
using VoiceCommentsExtension.Commands.BarCommands;

namespace VoiceCommentsExtension.ViewModels
{
    public class BarViewModel : BaseViewModel
    {
        public VoiceCommentViewModel VoiceCommentViewModel
        {
            get => _voiceCommentViewModel;
            set
            {
                _voiceCommentViewModel = value;

                OnPropertyChanged(nameof(VoiceCommentViewModel));
            }
        }
        private VoiceCommentViewModel _voiceCommentViewModel;

        public long Bytes
        {
            get => _bytes;
            set
            {
                _bytes = value;

                OnPropertyChanged(nameof(Bytes));
            }
        }
        private long _bytes;

        public double Value
        {
            get => _value;
            set
            {
                _value = value;

                OnPropertyChanged(nameof(Value));
            }
        }
        private double _value;

        public ICommand RewindPlayingCommand { get; set; }

        protected override void InitializeCommands()
        {
            RewindPlayingCommand = new RewindPlayingCommand();
        }
    }
}