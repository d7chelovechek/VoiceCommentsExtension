using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace VoiceCommentsExtension.Services
{
    public class PlayerService
    {
        public string FilePath { get; private set; }

        public double _totalMilliseconds;

        public PlayerService(string filePath)
        {
            FilePath = filePath;
        }

        public List<double> InitializeBars(int count)
        {
            var bars = new List<double>();

            using (var audioFileReader = new AudioFileReader(FilePath))
            {
                _totalMilliseconds =
                    audioFileReader.TotalTime.TotalMilliseconds;

                int samplesPerSection = 
                    (int)(audioFileReader.Length / 
                    sizeof(float) / count);

                byte[] buffer = new byte[samplesPerSection * sizeof(float)];

                for (var section = 0; section < count; section++)
                {
                    if (audioFileReader.Read(buffer, 0, buffer.Length) 
                            is int bytesRead &&
                        bytesRead is 0)
                    {
                        break;
                    }

                    var maxValue = 0d;
                    for (var index = 0; index < bytesRead; index += sizeof(float))
                    {
                        maxValue = Math.Max(maxValue, BitConverter.ToSingle(buffer, index));
                    }

                    bars.Add(Math.Max(0.2d, Math.Abs(maxValue)));
                }
            }

            return bars;
        }

        public double GetTotalMilliseconds() 
        { 
            return _totalMilliseconds;
        }
    }
}