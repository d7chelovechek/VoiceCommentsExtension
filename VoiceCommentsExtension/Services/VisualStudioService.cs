using EnvDTE;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Formatting;
using System.IO;
using System.Windows.Media;
using VoiceCommentsExtension.Views;
using VoiceCommentsExtension.WorkLayer;

namespace VoiceCommentsExtension.Services
{
    public static class VisualStudioService
    {
        public static string GetCurrentEditorFontFamily()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (ServiceProvider.GlobalProvider.GetService(typeof(DTE)) is DTE dte &&
                dte.Properties["FontsAndColors", "TextEditor"]
                    .Item("FontFamily").Value is string fontFamily)
            {
                return fontFamily;
            }

            return "Cascadia Code";
        }

        public static double GetCurrentEditorFontSize()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (ServiceProvider.GlobalProvider.GetService(typeof(DTE)) is DTE dte &&
                dte.Properties["FontsAndColors", "TextEditor"]
                    .Item("FontSize").Value is double fontSize)
            {
                return fontSize;
            }

            return 12;
        }

        public static string GetVoicesDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (Package.GetGlobalService(typeof(DTE)) is DTE dte && 
                dte.Solution is not null && 
                !string.IsNullOrWhiteSpace(dte.Solution.FullName))
            {
                string solutionPath = Path.GetDirectoryName(dte.Solution.FullName);
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

        public static void UpdateTheme(VoiceCommentsLayer layer, VoiceCommentView view)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (ServiceProvider.GlobalProvider.GetService(typeof(SVsUIShell)) is IVsUIShell5 vsUIShell)
            {
                view.ViewModel.Background = new SolidColorBrush(
                    vsUIShell.GetThemedWPFColor(EnvironmentColors.AccentMediumColorKey));
                view.ViewModel.Accent = new SolidColorBrush(
                    vsUIShell.GetThemedWPFColor(EnvironmentColors.AccentDarkColorKey));

                if (layer.ClassificationFormatMap?.GetClassificationFormatMap(layer.View) 
                        is IClassificationFormatMap classificationFormatMap &&
                    layer.ClassificationTypeRegistry?.GetClassificationType("comment") 
                        is IClassificationType commentClassificationType &&
                    classificationFormatMap.GetTextProperties(commentClassificationType) 
                        is TextFormattingRunProperties properties)
                {
                    view.ViewModel.Foreground = properties.ForegroundBrush as SolidColorBrush;
                    
                    view.FontFamily = new FontFamily(GetCurrentEditorFontFamily());
                    view.FontSize = GetCurrentEditorFontSize() * 4 / 5;
                }
            }
        }
    }
}