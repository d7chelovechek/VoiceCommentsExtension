using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using System.Threading.Tasks;

namespace VoiceCommentsExtension.Commands
{
    [Command(PackageGuids.VoiceCommentsExtensionPackageString, PackageIds.PasteVoiceCommentCommandId)]
    public class PasteVoiceCommentCommand : BaseCommand<PasteVoiceCommentCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            DocumentView document = await VS.Documents.GetActiveDocumentViewAsync();

            if (document?.TextView.Caret.Position.BufferPosition is SnapshotPoint position)
            {
                document?.TextBuffer.Insert(position, "Command is working");
            }
        }
    }
}