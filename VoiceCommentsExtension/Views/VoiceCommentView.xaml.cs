using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using VoiceCommentsExtension.ViewModels;

namespace VoiceCommentsExtension.Views
{
    public partial class VoiceCommentView : UserControl, IDisposable
    {
        public VoiceCommentViewModel ViewModel { get; private set; }

        private bool _isDisposed;

        public VoiceCommentView()
        {
            InitializeComponent();

            ViewModel = DataContext as VoiceCommentViewModel;

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            PreviewMouseDown += VoiceCommentView_PreviewMouseDown;
        }

        private void VoiceCommentView_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.OriginalSource is not FrameworkElement element ||
                element.TemplatedParent is not Button)
            {
                e.Handled = true;
                
                return;
            }
        }

        private void UnsubscribeFromEvents()
        {
            PreviewMouseDown -= VoiceCommentView_PreviewMouseDown;
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
    }
}