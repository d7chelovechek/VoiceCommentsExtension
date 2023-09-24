using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Threading;
using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using VoiceCommentsExtension.Services;
using VoiceCommentsExtension.ViewModels;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace VoiceCommentsExtension.Windows
{
    public partial class RecorderWindow : DialogWindow, IDisposable
    {
        public RecorderViewModel ViewModel { get; private set; }

        private bool _isDisposed;

        public RecorderWindow()
        {
            InitializeComponent();

            ViewModel = DataContext as RecorderViewModel;

            SubscribeToEvents();

            SetWindowPosition();
            SetWindowFont();
        }

        private void SubscribeToEvents()
        {
            ViewModel.RecordingService.CloseWindowNeeded += OnCloseWindowNeeded;
            ViewModel.ClosingWindow += OnClosingWindow;
        }

        private void OnClosingWindow()
        {
            Visibility = System.Windows.Visibility.Collapsed;
            Hide();
        }

        public void UnsubscribeFromEvents()
        {
            ViewModel.RecordingService.CloseWindowNeeded -= OnCloseWindowNeeded;
            ViewModel.ClosingWindow -= OnClosingWindow;
        }

        private void OnCloseWindowNeeded()
        {
            UnsubscribeFromEvents();

            Close();
        }

        private void SetWindowFont()
        {
            string font = VisualStudioService.GetCurrentEditorFontFamily();

            FontFamily = new FontFamily(font);
        }

        public void SetWindowPosition()
        {
            var helper = new WindowInteropHelper(this);
            var screen = Screen.FromHandle(helper.Handle);

            Left = Math.Max(0, Math.Min(
                System.Windows.Forms.Cursor.Position.X,
                screen.WorkingArea.Width - Width));
            Top = Math.Max(0, Math.Min(
                System.Windows.Forms.Cursor.Position.Y,
                screen.WorkingArea.Height - Height));
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;

            UnsubscribeFromEvents();

            if (ViewModel.IsRecordingStarted)
            {
                ViewModel.RecordingService.DeleteFileNeeded = true;
                ViewModel.RecordingService.StopRecording();
            }
        }
    }
}