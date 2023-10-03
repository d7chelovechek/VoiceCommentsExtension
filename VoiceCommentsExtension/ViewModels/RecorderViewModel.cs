using System;
using System.Windows.Input;
using System.Windows.Threading;
using VoiceCommentsExtension.Commands.RecorderCommands;
using VoiceCommentsExtension.Services;

namespace VoiceCommentsExtension.ViewModels
{
    public class RecorderViewModel : BaseViewModel, IDisposable
    {
        public bool IsRecording
        {
            get => Recorder.IsRecording;
            set
            {
                Recorder.IsRecording = value;

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

        public double MillisecondsElapsed
        {
            get => _millisecondsElapsed;
            set
            {
                _millisecondsElapsed = value;

                OnPropertyChanged(nameof(MillisecondsElapsed));
            }
        }
        private double _millisecondsElapsed;

        public DispatcherTimer Timer { get; private set; }

        public bool? RecordingResult { get; set; }

        public RecorderService Recorder { get; private set; }

        public ICommand CancelRecordingCommand { get; private set; }
        public ICommand StopRecordingCommand { get; private set; }
        public ICommand SaveVoiceCommentCommand { get; private set; }
        public ICommand StartRecordingCommand { get; private set; }

        private const int _interval = 100;

        private bool _isDisposed;

        public RecorderViewModel()
        {
            Recorder = new RecorderService();

            Timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(_interval)
            };
            Timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MillisecondsElapsed += _interval;
        }

        protected override void InitializeCommands()
        {
            base.InitializeCommands();

            CancelRecordingCommand = new CancelRecordingCommand();
            StopRecordingCommand = new StopRecordingCommand();
            SaveVoiceCommentCommand = new SaveVoiceCommentCommand();
            StartRecordingCommand = new StartRecordingCommand();
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;

            UnsubscribeFromEvents();
        }

        private void UnsubscribeFromEvents()
        {
            Timer.Tick -= Timer_Tick;
        }
    }
}