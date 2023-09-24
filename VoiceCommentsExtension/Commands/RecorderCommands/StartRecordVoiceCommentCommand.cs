﻿using VoiceCommentsExtension.ViewModels;

namespace VoiceCommentsExtension.Commands.RecorderCommands
{
    public class StartRecordVoiceCommentCommand : BaseTypedCommand<RecorderViewModel>
    {
        public override void Execute(RecorderViewModel viewModel)
        {
            viewModel.Timer.Start();

            if (!viewModel.IsRecordingStarted)
            {
                viewModel.IsRecordingStarted = true;

                viewModel.RecordingService.StartRecording();
            }

            viewModel.IsRecording = true;
        }
    }
}