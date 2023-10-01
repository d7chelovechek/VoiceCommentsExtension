using VoiceCommentsExtension.ViewModels;

namespace VoiceCommentsExtension.Commands.VoiceCommentCommands
{
    public class StopPlayingCommand : BaseTypedCommand<VoiceCommentViewModel>
    {
        public override void Execute(VoiceCommentViewModel viewModel)
        {
            viewModel.Player.StopPlaying();
        }
    }
}