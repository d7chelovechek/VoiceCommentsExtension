using VoiceCommentsExtension.ViewModels;

namespace VoiceCommentsExtension.Commands.BarCommands
{
    public class RewindPlayingCommand : BaseTypedCommand<BarViewModel>
    {
        public override void Execute(BarViewModel viewModel)
        {
            viewModel.VoiceCommentViewModel.Player.RewindPlaying(viewModel.Bytes);
            viewModel.VoiceCommentViewModel.CurrentBytes = viewModel.Bytes;
        }
    }
}