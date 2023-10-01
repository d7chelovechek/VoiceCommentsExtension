using NAudio.Wave;
using System;
using System.IO;

namespace VoiceCommentsExtension.Services
{
    public class RecorderService : IDisposable
    {
        public string FilePath { get; private set; }

        private readonly WaveInEvent _waveIn;
        private readonly WaveFileWriter _writer;

        public bool IsRecording { get; set; }
        public bool DeleteFileNeeded { get; set; }

        public event Action CloseWindowNeeded;

        private bool _isDisposed;

        private bool _isRecordingStarted;

        public RecorderService()
        {
            FilePath = TryGetFilePath(Guid.NewGuid().ToString().Split('-')[4]);

            if (FilePath is null)
            {
                throw new NullReferenceException();
            }

            _waveIn = new WaveInEvent()
            {
                WaveFormat = new WaveFormat(44100, 16, 1)
            };

            _writer = new WaveFileWriter(FilePath, _waveIn.WaveFormat);
        }

        private string TryGetFilePath(string fileName)
        {
            string filePath = $"{VisualStudioService.GetVoicesDirectory()}\\{fileName}.wav";

            if (File.Exists(filePath)) 
            {
                return TryGetFilePath(Guid.NewGuid().ToString().Split('-')[4]);
            }

            return filePath;
        }

        private void SubscribeToEvents()
        {
            _waveIn.DataAvailable += Recorder_DataAvailable;
            _waveIn.RecordingStopped += Recorder_RecordingStopped;
        }

        private void UnsubscribeFromEvents()
        {
            _waveIn.DataAvailable -= Recorder_DataAvailable;
            _waveIn.RecordingStopped -= Recorder_RecordingStopped;
        }

        public void StartRecording()
        {
            if (_isRecordingStarted)
            {
                return;
            }
            _isRecordingStarted = true;

            SubscribeToEvents();

            _waveIn.StartRecording();
        }

        public void StopRecording() 
        {
            if (!_isRecordingStarted)
            {
                return;
            }
            _isRecordingStarted = false;

            _waveIn.StopRecording();
        }

        private void Recorder_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (!IsRecording)
            {
                return;
            }
            
            _writer.Write(e.Buffer, 0, e.BytesRecorded);
        }

        private void Recorder_RecordingStopped(object sender, StoppedEventArgs e)
        {
            Dispose();
        }

        public void InvokeCloseWindowNeededEvent()
        {
            CloseWindowNeeded?.Invoke();
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;

            UnsubscribeFromEvents();

            _waveIn.Dispose();

            _writer.Close();
            _writer.Dispose();

            if (DeleteFileNeeded && File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }

            InvokeCloseWindowNeededEvent();
        }
    }
}