using VoiceCommentsExtension.ViewModels;

namespace VoiceCommentsExtension.Commands.RecorderCommands
{
    public class StartRecordingCommand : BaseTypedCommand<RecorderViewModel>
    {
        public override void Execute(RecorderViewModel viewModel)
        {
            viewModel.Timer.Start();

            if (!viewModel.IsRecordingStarted)
            {
                viewModel.IsRecordingStarted = true;

                viewModel.Recorder.StartRecording();
            }

            viewModel.IsRecording = true;
        }
    }
}