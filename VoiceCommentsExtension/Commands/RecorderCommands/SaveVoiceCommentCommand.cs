using VoiceCommentsExtension.ViewModels;

namespace VoiceCommentsExtension.Commands.RecorderCommands
{
    public class SaveVoiceCommentCommand : BaseTypedCommand<RecorderViewModel>
    {
        public override void Execute(RecorderViewModel viewModel)
        {
            viewModel.InvokeClosingWindowEvent();
            viewModel.Timer.Stop();

            viewModel.RecordingResult = true;

            viewModel.Recorder.StopRecording();
        }
    }
}