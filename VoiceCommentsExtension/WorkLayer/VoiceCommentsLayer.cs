using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VoiceCommentsExtension.Services;
using VoiceCommentsExtension.Views;

namespace VoiceCommentsExtension.WorkLayer
{
    public class VoiceCommentsLayer
    {
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

            foreach (VoiceCommentView view in _voiceComments.ToList())
            {
                RemoveVoiceComment(view);
            }
        }

        private void View_LayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!e.VerticalTranslation && !e.HorizontalTranslation)
            {
                VisualStudioService.UpdateTheme(this, _voiceComments.ToArray());
            }

            if (e.NewOrReformattedLines?.Any() is not true)
            {
                return;
            }

            foreach (ITextViewLine line in e.NewOrReformattedLines)
            {
                try
                {
                    if (CommentsService.TryMatch(
                            _contentTypeName,
                            line.Extent.GetText(),
                            out string filePath)
                                is int matchIndex &&
                        matchIndex >= 0 &&
                        !string.IsNullOrWhiteSpace(filePath) &&
                        File.Exists(filePath))
                    {
                        TryDrawVoiceCommentListener(
                            line,
                            matchIndex,
                            filePath);
                    }
                }
                catch { }
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
            ITextViewLine voiceCommentLine,
            int matchIndex,
            string filePath)
        {
            var span = new SnapshotSpan(
                View.TextSnapshot, 
                Span.FromBounds(
                    voiceCommentLine.Extent.Start.Position + matchIndex,
                    voiceCommentLine.Start + voiceCommentLine.Extent.Length));
            Geometry geometry = View.TextViewLines.GetMarkerGeometry(span);

            if (geometry is null)
            {
                return;
            }

            if (_voiceComments.FirstOrDefault(v => filePath.Equals(v.ViewModel?.Player?.FilePath))
                    is not VoiceCommentView view)
            {
                view = new VoiceCommentView();
                view.ViewModel.InitializePlayer(filePath);

                _voiceComments.Add(view);
                VisualStudioService.UpdateTheme(this, view);
            }

            view.InvalidateLayout(View, voiceCommentLine, geometry);

            _layer.AddAdornment(
                AdornmentPositioningBehavior.TextRelative,
                voiceCommentLine.Extent,
                null,
                view,
                null);
        }
    }
}