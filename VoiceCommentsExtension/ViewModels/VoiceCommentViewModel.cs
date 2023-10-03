using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using VoiceCommentsExtension.Commands.VoiceCommentCommands;
using VoiceCommentsExtension.Services;

namespace VoiceCommentsExtension.ViewModels
{
    public class VoiceCommentViewModel : BaseViewModel, IDisposable
    {
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;

                OnPropertyChanged(nameof(IsPlaying));
            }
        }
        private bool _isPlaying;

        public SolidColorBrush Background
        {
            get => _background;
            set
            {
                _background = value;

                OnPropertyChanged(nameof(Background));
            }
        }
        private SolidColorBrush _background;

        public SolidColorBrush Foreground
        {
            get => _foreground;
            set
            {
                _foreground = value;

                OnPropertyChanged(nameof(Foreground));
            }
        }
        private SolidColorBrush _foreground;

        public PlayerService Player { get; private set; }

        public DispatcherTimer Timer { get; private set; }

        public double TotalMilliseconds
        {
            get => _totalMilliseconds;
            set
            {
                _totalMilliseconds = value;

                OnPropertyChanged(nameof(TotalMilliseconds));
            }
        }
        private double _totalMilliseconds;

        public double CurrentMilliseconds
        {
            get => _currentMilliseconds;
            set
            {
                _currentMilliseconds = value;

                OnPropertyChanged(nameof(CurrentMilliseconds));
            }
        }
        private double _currentMilliseconds;
        
        public long CurrentBytes
        {
            get => _currentBytes;
            set
            {
                _currentBytes = value;

                OnPropertyChanged(nameof(CurrentBytes));
            }
        }
        private long _currentBytes;

        public ICommand StartPlayingCommand { get; private set; }
        public ICommand StopPlayingCommand { get; private set; }

        public ObservableCollection<BarViewModel> Bars { get; private set; }

        private const int _interval = 100;

        private bool _isDisposed;

        public VoiceCommentViewModel()
        {
            Timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(_interval)
            };
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentBytes = Player.GetCurrentPosition();
            CurrentMilliseconds = Player.GetCurrentMilliseconds();
        }

        protected override void InitializeCommands()
        {
            base.InitializeCommands();

            StartPlayingCommand = new StartPlayingCommand();
            StopPlayingCommand = new StopPlayingCommand();
        }

        public void InitializePlayer(string filePath)
        {
            Bars = new ObservableCollection<BarViewModel>();

            Player = new PlayerService(filePath);
            SubscribeToEvents();

            List<double> bars = Player.InitializeBars(out int bytesPerBar);
            TotalMilliseconds = Player.TotalMilliseconds;

            for (var index = 0; index < bars.Count; index++)
            {
                Bars.Add(new BarViewModel()
                {
                    Bytes = (index + 1) * (long)bytesPerBar,
                    Value = bars[index],
                    VoiceCommentViewModel = this
                });
            }
        }

        private void SubscribeToEvents()
        {
            Player.PlayingStopped += Player_PlayingStopped;
            Timer.Tick += Timer_Tick;
        }

        private void Player_PlayingStopped()
        {
            IsPlaying = false;
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;

            UnsubscribeFromEvents();

            Timer.Stop();
        }

        private void UnsubscribeFromEvents()
        {
            Player.PlayingStopped -= Player_PlayingStopped;
            Timer.Tick -= Timer_Tick;
        }
    }
}