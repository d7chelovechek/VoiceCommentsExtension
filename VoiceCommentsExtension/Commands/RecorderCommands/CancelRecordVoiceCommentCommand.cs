using VoiceCommentsExtension.ViewModels;

namespace VoiceCommentsExtension.Commands.RecorderCommands
{
    public class CancelRecordVoiceCommentCommand : BaseTypedCommand<RecorderViewModel>
    {
        public override void Execute(RecorderViewModel viewModel)
        {
            viewModel.InvokeClosingWindowEvent();
            viewModel.Timer.Stop();

            viewModel.RecordingResult = false;
            if (!viewModel.IsRecordingStarted)
            {
                viewModel.RecordingService.InvokeCloseWindowNeededEvent();

                return;
            }
            
            viewModel.RecordingService.DeleteFileNeeded = true;
            viewModel.RecordingService.StopRecording();
        }
    }
}