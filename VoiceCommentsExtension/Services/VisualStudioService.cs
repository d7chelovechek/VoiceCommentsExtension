using EnvDTE;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Formatting;
using System;
using System.IO;
using System.Windows.Media;
using VoiceCommentsExtension.Views;
using VoiceCommentsExtension.WorkLayer;

namespace VoiceCommentsExtension.Services
{
    public static class VisualStudioService
    {
        private readonly static DTE _dte;
        private readonly static IVsUIShell5 _vsUiShell5;

        static VisualStudioService()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (ServiceProvider.GlobalProvider.GetService(typeof(DTE)) is DTE dte &&
                ServiceProvider.GlobalProvider.GetService(typeof(SVsUIShell)) 
                    is IVsUIShell5 vsUIShell5 &&
                Package.GetGlobalService(typeof(SVsSettingsManager))
                    is IVsSettingsManager vsSettingsManager)
            {
                _dte = dte;
                _vsUiShell5 = vsUIShell5;
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public static string GetCurrentEditorFontFamily()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (_dte.Properties["FontsAndColors", "TextEditor"]
                    .Item("FontFamily").Value is string fontFamily)
            {
                return fontFamily;
            }

            return "Cascadia Code";
        }

        public static double GetCurrentEditorFontSize()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (_dte.Properties["FontsAndColors", "TextEditor"]
                    .Item("FontSize").Value is double fontSize)
            {
                return fontSize;
            }

            return 12;
        }

        public static string GetVoicesDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (_dte.Solution is not null && 
                    !string.IsNullOrWhiteSpace(_dte.Solution.FullName))
            {
                string solutionPath = Path.GetDirectoryName(_dte.Solution.FullName);
                string voicesPath = $"{solutionPath}\\.voice-comments";

                Directory.CreateDirectory(voicesPath);
                File.SetAttributes(
                    voicesPath, 
                    File.GetAttributes(voicesPath) | FileAttributes.Hidden);

                return voicesPath;
            }
            else
            {
                return null;
            }
        }

        public static void UpdateTheme(VoiceCommentsLayer layer, params VoiceCommentView[] views)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            SolidColorBrush background = Brushes.Gray;
            SolidColorBrush foreground = Brushes.Green;

            var fontFamily = new FontFamily("Cascadia Code");
            var fontSize = 12d;

            if (layer.ClassificationFormatMap?.GetClassificationFormatMap(layer.View)
                    is IClassificationFormatMap classificationFormatMap &&
                layer.ClassificationTypeRegistry?.GetClassificationType("comment")
                    is IClassificationType commentClassificationType &&
                classificationFormatMap.GetTextProperties(commentClassificationType)
                    is TextFormattingRunProperties properties)
            {
                background = new SolidColorBrush(
                    _vsUiShell5.GetThemedWPFColor(EnvironmentColors.AccentMediumColorKey));
                foreground = properties.ForegroundBrush as SolidColorBrush;

                fontFamily = new FontFamily(GetCurrentEditorFontFamily());
                fontSize = GetCurrentEditorFontSize();
            }

            foreach (VoiceCommentView view in views) 
            {
                view.ViewModel.Background = background;
                view.ViewModel.Foreground = foreground;

                view.FontFamily = fontFamily;
                view.FontSize = fontSize;
            }
        }
    }
}