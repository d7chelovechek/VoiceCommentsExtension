using Community.VisualStudio.Toolkit;
using System;
using System.Windows;
using VoiceCommentsExtension.Services;
using VoiceCommentsExtension.Windows;

namespace VoiceCommentsExtension.Commands
{
    [Command(PackageGuids.VoiceCommentsExtensionPackageString, PackageIds.OpenRecorderWindowCommandId)]
    public class OpenRecorderWindowCommand : BaseCommand<OpenRecorderWindowCommand>
    {
        protected override void Execute(object sender, EventArgs e)
        {
            var dialog = new RecorderWindow();
            Point position = WindowsService.GetStartWindowPosition(dialog);

            dialog.Left = position.X;
            dialog.Top = position.Y;

            dialog.ShowDialog();
        }
    }
}