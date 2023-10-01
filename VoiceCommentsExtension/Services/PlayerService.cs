using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VoiceCommentsExtension.Services
{
    public class PlayerService : IDisposable
    {
        public string FilePath { get; private set; }

        public double TotalMilliseconds { get; private set; }

        private readonly WaveStream _reader;
        private readonly WaveOutEvent _waveOut;

        private const int _parts = 32;

        private bool _isDisposed;

        private bool _isPlayingStarted;
        private long _currentPosition;

        public event Action PlayingStopped;

        public PlayerService(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new NotImplementedException();
            }

            FilePath = filePath;

            _reader = new AudioFileReader(filePath);

            _waveOut = new WaveOutEvent();
            _waveOut.Init(_reader);

            SubscribeToEvents();
        }

        private void Player_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            PlayingStopped?.Invoke();

            if (_reader.Position >= _reader.Length)
            {
                _reader.Position = _currentPosition = 0;
            }
            else
            {
                _currentPosition = _reader.Position;
            }

            _isPlayingStarted = false;
        }

        private void SubscribeToEvents()
        {
            _waveOut.PlaybackStopped += Player_PlaybackStopped;
        }

        private void UnsubscribeFromEvents()
        {
            _waveOut.PlaybackStopped -= Player_PlaybackStopped;
        }

        public List<double> InitializeBars(out int bytesPerBar)
        {
            var bars = new List<double>();
            bytesPerBar = 0;

            TotalMilliseconds =
                _reader.TotalTime.TotalMilliseconds;

            int channels = _reader.WaveFormat.Channels;

            int samplesPerSection = (int)_reader.Length / (channels * sizeof(float)) / _parts;

            bytesPerBar = samplesPerSection * channels * sizeof(float);
            byte[] buffer = new byte[bytesPerBar];

            for (var section = 0; section < _parts; section++)
            {
                if (_reader.Read(buffer, 0, buffer.Length)
                        is int bytesRead &&
                    bytesRead is 0)
                {
                    break;
                }

                var localMaxValue = 0d;
                for (var index = 0; index < bytesRead; index += sizeof(float))
                {
                    localMaxValue = Math.Max(localMaxValue, BitConverter.ToSingle(buffer, index));
                }

                bars.Add(Math.Max(0.2d, Math.Abs(localMaxValue)));
            }
            
            _reader.Position = _currentPosition = 0;

            double maxValue = bars.Max();
            return bars.Select(b => b / maxValue).ToList();
        }

        public void StartPlaying()
        {
            if (_isPlayingStarted)
            {
                return;
            }
            _isPlayingStarted = true;

            _reader.Position = _currentPosition;
            _waveOut.Play();
        }

        public long GetCurrentPosition()
        {
            return _reader.Position;
        }

        public double GetCurrentMilliseconds()
        {
            return _reader.CurrentTime.TotalMilliseconds;
        }

        public void StopPlaying()
        {
            if (!_isPlayingStarted)
            {
                return;
            }
            _isPlayingStarted = false;

            _waveOut.Stop();
            _currentPosition = _reader.Position;
        }

        public void RewindPlaying(long bytes)
        {
            _reader.Seek(bytes, SeekOrigin.Begin);
            _currentPosition = bytes;
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;

            UnsubscribeFromEvents();

            if (_isPlayingStarted)
            {
                _waveOut.Stop();
            }
            _waveOut.Dispose();

            _reader.Close();
            _reader.Dispose();
        }
    }
}