﻿using VoiceCommentsExtension.ViewModels;

namespace VoiceCommentsExtension.Commands.RecorderCommands
{
    public class StopRecordingCommand : BaseTypedCommand<RecorderViewModel>
    {
        public override void Execute(RecorderViewModel viewModel)
        {
            viewModel.Timer.Stop();

            viewModel.IsRecording = false;
        }
    }
}