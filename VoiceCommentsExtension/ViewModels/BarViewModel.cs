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

        public int Index
        {
            get => _index;
            set
            {
                _index = value;

                OnPropertyChanged(nameof(Index));
            }
        }
        private int _index;

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
    }
}