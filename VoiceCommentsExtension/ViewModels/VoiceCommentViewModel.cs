using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Media;
using VoiceCommentsExtension.Services;

namespace VoiceCommentsExtension.ViewModels
{
    public class VoiceCommentViewModel : BaseViewModel
    {
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

        public SolidColorBrush Accent
        {
            get => _accent;
            set
            {
                _accent = value;

                OnPropertyChanged(nameof(Accent));
            }
        }
        private SolidColorBrush _accent;

        public bool IsInEditor { get; set; }

        public PlayerService Player { get; private set; }

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

        public ObservableCollection<BarViewModel> Bars { get; private set; }

        public void InitializePlayer(string filePath)
        {
            Bars = new ObservableCollection<BarViewModel>();

            Player = new PlayerService(filePath);

            List<double> bars = Player.InitializeBars(32);
            TotalMilliseconds = Player.GetTotalMilliseconds();

            for (var index = 0; index < bars.Count; index++) 
            {
                Bars.Add(new BarViewModel()
                {
                    Index = index,
                    Value = bars[index],
                    VoiceCommentViewModel = this
                });
            }
        }
    }
}