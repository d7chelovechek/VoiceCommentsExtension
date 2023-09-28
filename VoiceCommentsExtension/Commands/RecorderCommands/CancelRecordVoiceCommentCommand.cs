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
                viewModel.Recorder.InvokeCloseWindowNeededEvent();

                return;
            }
            
            viewModel.Recorder.DeleteFileNeeded = true;
            viewModel.Recorder.StopRecording();
        }
    }
}