using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.IO;

namespace VoiceCommentsExtension.Services
{
    public static class VisualStudioService
    {
        public static string GetCurrentEditorFontFamily()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var serviceProvider = ServiceProvider.GlobalProvider;
            if (serviceProvider.GetService(typeof(DTE)) is DTE dte)
            {
                return dte.Properties["FontsAndColors", "TextEditor"].Item("FontFamily").Value as string;
            }

            return "Cascadia Code";
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
    }
}