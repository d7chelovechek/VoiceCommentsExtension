using NAudio.Wave;
using System;
using System.IO;

namespace VoiceCommentsExtension.Services
{
    public class RecordingService
    {
        public string FilePath { get; private set; }

        public WaveInEvent Recorder { get; private set; }
        public WaveFileWriter WaveFileWriter { get; private set; }

        public bool IsRecording { get; set; }
        public bool DeleteFileNeeded { get; set; }

        public event Action CloseWindowNeeded;

        public RecordingService()
        {
            FilePath = $"{VisualStudioService.GetVoicesDirectory()}\\{Guid.NewGuid()}.wav";

            if (FilePath is null)
            {
                throw new NullReferenceException();
            }

            Recorder = new WaveInEvent()
            {
                WaveFormat = new WaveFormat(44100, 16, 1)
            };
        }

        private void SubscribeToEvents()
        {
            Recorder.DataAvailable += Recorder_DataAvailable;
            Recorder.RecordingStopped += Recorder_RecordingStopped;
        }

        private void UnsubscribeFromEvents()
        {
            Recorder.DataAvailable -= Recorder_DataAvailable;
            Recorder.RecordingStopped -= Recorder_RecordingStopped;
        }

        public void StartRecording()
        {
            SubscribeToEvents();

            WaveFileWriter = new WaveFileWriter(FilePath, Recorder.WaveFormat);
            Recorder.StartRecording();
        }

        public void StopRecording() 
        {
            Recorder.StopRecording();
        }

        private void Recorder_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (!IsRecording)
            {
                return;
            }
            
            WaveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);
        }

        private void Recorder_RecordingStopped(object sender, StoppedEventArgs e)
        {
            WaveFileWriter.Close();
            WaveFileWriter.Dispose();

            if (DeleteFileNeeded && File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }

            UnsubscribeFromEvents();
            InvokeCloseWindowNeededEvent();
        }

        public void InvokeCloseWindowNeededEvent()
        {
            CloseWindowNeeded?.Invoke();
        }
    }
}