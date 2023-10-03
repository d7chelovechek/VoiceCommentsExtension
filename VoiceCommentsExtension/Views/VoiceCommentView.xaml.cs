using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        public void InvalidateLayout(IWpfTextView view, ITextViewLine line, Geometry geometry)
        {
            double newHeight = view.LineHeight * 3;

            Height = newHeight;
            Width = geometry.Bounds.Right - geometry.Bounds.Left;
            PlayerColumn.Width = new GridLength(newHeight - 10, GridUnitType.Pixel);

            Canvas.SetLeft(this, geometry.Bounds.Left);
            Canvas.SetTop(this, line.TextTop - view.LineHeight);
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