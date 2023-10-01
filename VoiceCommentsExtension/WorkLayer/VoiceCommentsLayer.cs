using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using VoiceCommentsExtension.Services;
using VoiceCommentsExtension.Views;

namespace VoiceCommentsExtension.WorkLayer
{
    public class VoiceCommentsLayer
    {
        public ITextDocumentFactoryService TextDocumentFactory { get; set; }
        public IClassificationFormatMapService ClassificationFormatMap { get; set; }
        public IClassificationTypeRegistryService ClassificationTypeRegistry { get; set; }

        public IWpfTextView View { get; private set; }
        private readonly IAdornmentLayer _layer;

        private string _contentTypeName;

        private readonly System.Collections.Generic.List<VoiceCommentView> _voiceComments;

        public VoiceCommentsLayer(IWpfTextView view)
        {
            View = view;
            _layer = view.GetAdornmentLayer("VoiceCommentsLayer");

            _contentTypeName = View.TextBuffer.ContentType.TypeName;

            _voiceComments = new System.Collections.Generic.List<VoiceCommentView>();

            SubcribeToEvents();
        }

        private void SubcribeToEvents()
        {
            View.LayoutChanged += View_LayoutChanged;
            View.Closed += View_Closed;
            View.TextBuffer.ContentTypeChanged += TextBuffer_ContentTypeChanged;
        }

        private void TextBuffer_ContentTypeChanged(object sender, ContentTypeChangedEventArgs e)
        {
            _contentTypeName = e.AfterContentType.TypeName;
        }

        private void UnsubcribeToEvents()
        {
            View.LayoutChanged -= View_LayoutChanged;
            View.Closed -= View_Closed;
            View.TextBuffer.ContentTypeChanged -= TextBuffer_ContentTypeChanged;
        }

        private void View_Closed(object sender, EventArgs e)
        {
            UnsubcribeToEvents();
        }

        private void View_LayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _voiceComments.ForEach(v => v.ViewModel.IsInEditor = false);

            for (var index = 0; index < View.TextViewLines.Count - 1; index++)
            {
                try
                {
                    if (View.TextViewLines[index] is ITextViewLine line &&
                        View.TextViewLines[index + 1] is ITextViewLine nextLine &&
                        CommentsService.TryMatch(
                            _contentTypeName,
                            line.Extent.GetText(),
                            nextLine.Extent.GetText(),
                            out string filePath)
                                is int matchIndex &&
                        matchIndex >= 0 &&
                        !string.IsNullOrWhiteSpace(filePath) &&
                        File.Exists(filePath))
                    {
                        TryDrawVoiceCommentListener(
                            line,
                            line.Length > nextLine.Length ? line : nextLine,
                            matchIndex,
                            filePath);
                    }
                }
                catch { }
            }

            foreach (VoiceCommentView view 
                in _voiceComments.Where(v => !v.ViewModel.IsInEditor).ToList())
            {
                RemoveVoiceComment(view);
            }
        }

        private void RemoveVoiceComment(VoiceCommentView view)
        {
            _layer.RemoveAdornment(view);
            _voiceComments.Remove(view);

            view.ViewModel.Player.Dispose();
            view.ViewModel.Dispose();
            view.Dispose();
        }

        private void TryDrawVoiceCommentListener(
            ITextViewLine line,
            ITextViewLine longestLine,
            int matchIndex,
            string filePath)
        {
            var span = new SnapshotSpan(
                View.TextSnapshot, 
                Span.FromBounds(
                    longestLine.Extent.Start.Position + matchIndex,
                    longestLine.Start + longestLine.Extent.Length));
            Geometry geometry = View.TextViewLines.GetMarkerGeometry(span);

            if (geometry is null)
            {
                return;
            }

            if (_voiceComments.FirstOrDefault(v => filePath.Equals(v.ViewModel?.Player?.FilePath))
                    is VoiceCommentView view)
            {
                _layer.RemoveAdornment(view);
            }
            else
            {
                view = new VoiceCommentView();
                view.ViewModel.InitializePlayer(filePath);

                _voiceComments.Add(view);
            }

            view.Height = View.LineHeight * 2;
            view.Width = geometry.Bounds.Right - geometry.Bounds.Left;
            VisualStudioService.UpdateTheme(this, view);

            Canvas.SetLeft(view, geometry.Bounds.Left);
            Canvas.SetTop(view, line.TextTop);

            _layer.AddAdornment(
                AdornmentPositioningBehavior.TextRelative,
                line.Extent,
                null,
                view,
                null);

            view.ViewModel.IsInEditor = true;
        }
    }
}