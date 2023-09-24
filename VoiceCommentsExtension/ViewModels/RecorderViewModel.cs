using System;
using System.Windows.Input;
using System.Windows.Threading;
using VoiceCommentsExtension.Commands.RecorderCommands;
using VoiceCommentsExtension.Services;

namespace VoiceCommentsExtension.ViewModels
{
    public class RecorderViewModel : BaseViewModel
    {
        public bool IsRecording
        {
            get => RecordingService.IsRecording;
            set
            {
                RecordingService.IsRecording = value;

                OnPropertyChanged(nameof(IsRecording));
            }
        }

        public bool IsRecordingStarted
        {
            get => _recordingStarted;
            set
            {
                _recordingStarted = value;

                OnPropertyChanged(nameof(IsRecordingStarted));
            }
        }
        private bool _recordingStarted;

        public long MillisecondsElapsed
        {
            get => _millisecondsElapsed;
            set
            {
                _millisecondsElapsed = value;

                OnPropertyChanged(nameof(MillisecondsElapsed));
            }
        }
        private long _millisecondsElapsed;

        public DispatcherTimer Timer { get; set; }

        public bool? RecordingResult { get; set; }

        public RecordingService RecordingService { get; private set; }

        public ICommand CancelRecordVoiceCommentCommand { get; private set; }
        public ICommand PauseRecordVoiceCommentCommand { get; private set; }
        public ICommand SaveVoiceCommentCommand { get; private set; }
        public ICommand StartRecordVoiceCommentCommand { get; private set; }

        public event Action ClosingWindow;

        private const int _interval = 100;

        public RecorderViewModel()
        {
            RecordingService = new RecordingService();

            Timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(_interval)
            };
            Timer.Tick += Timer_Tick;
        }

        public void InvokeClosingWindowEvent()
        {
            ClosingWindow?.Invoke();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MillisecondsElapsed += _interval;
        }

        protected override void InitializeCommands()
        {
            CancelRecordVoiceCommentCommand = new CancelRecordVoiceCommentCommand();
            PauseRecordVoiceCommentCommand = new PauseRecordVoiceCommentCommand();
            SaveVoiceCommentCommand = new SaveVoiceCommentCommand();
            StartRecordVoiceCommentCommand = new StartRecordVoiceCommentCommand();
        }
    }
}