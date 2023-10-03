using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using VoiceCommentsExtension.Services;
using VoiceCommentsExtension.ViewModels;

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

            SetWindowFont();
        }

        private void SubscribeToEvents()
        {
            ViewModel.Recorder.CloseWindowNeeded += OnCloseWindowNeeded;
            SourceInitialized += RecorderWindow_SourceInitialized;
        }

        private void RecorderWindow_SourceInitialized(object sender, EventArgs e)
        {
            SetWindowPosition();
        }

        public void UnsubscribeFromEvents()
        {
            ViewModel.Recorder.CloseWindowNeeded -= OnCloseWindowNeeded;
            SourceInitialized -= RecorderWindow_SourceInitialized;
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

        private void SetWindowPosition()
        {
            double top = System.Windows.Forms.Cursor.Position.Y;
            double left = System.Windows.Forms.Cursor.Position.X;

            Screen screen = Screen.FromHandle(new WindowInteropHelper(this).Handle);

            if (left + Width > screen.WorkingArea.Width)
            {
                left -= Width;
            }
            if (top + Height > screen.WorkingArea.Height)
            {
                top -= Height;
            }

            Top = top;
            Left = left;
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
                ViewModel.Recorder.DeleteFileNeeded = true;
                ViewModel.Recorder.StopRecording();
            }
        }
    }
}