using Community.VisualStudio.Toolkit;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using System.IO;
using System.Threading.Tasks;
using VoiceCommentsExtension.Windows;

namespace VoiceCommentsExtension.ExtensionCommands
{
    [Command(PackageGuids.VoiceCommentsExtensionPackageString, PackageIds.RecordVoiceCommentCommandId)]
    public class OpenRecorderWindowCommand : BaseCommand<OpenRecorderWindowCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            DocumentView document = await VS.Documents.GetActiveDocumentViewAsync();

            if (document is null)
            {
                return;
            }

            SnapshotPoint point = document.TextView.Caret.Position.VirtualBufferPosition.Position;

            var dialog = new RecorderWindow();
            dialog.ShowDialog();

            switch (dialog.ViewModel.RecordingResult)
            {
                case true:
                    {
                        document?.TextBuffer.Insert(
                            point,
                            $"\r\n// <voice-comment:{Path.GetFileName(dialog.ViewModel.RecordingService.FilePath)}>");

                        if (ServiceProvider.GlobalProvider.GetService(typeof(DTE)) is DTE2 dte)
                        {
                            dte.ExecuteCommand("Edit.FormatDocument");
                        }

                        break;
                    }
                case null:
                    {
                        dialog.Dispose();

                        break;
                    }
            }
        }
    }
}