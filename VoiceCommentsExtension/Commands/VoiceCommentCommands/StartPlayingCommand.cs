using VoiceCommentsExtension.ViewModels;

namespace VoiceCommentsExtension.Commands.VoiceCommentCommands
{
    public class StartPlayingCommand : BaseTypedCommand<VoiceCommentViewModel>
    {
        public override void Execute(VoiceCommentViewModel viewModel)
        {
            viewModel.Player.StartPlaying();

            viewModel.IsPlaying = true;
        }
    }
}