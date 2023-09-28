using System;
using System.Globalization;
using System.Windows;

namespace VoiceCommentsExtension.Converters
{
    public class MillisecondsToStringViewConverter : BaseValueConverter
    {
        public override object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (parameter is not string stringParameter ||
                !bool.TryParse(
                    stringParameter, 
                    out bool needViewMilliseconds))
            {
                needViewMilliseconds = false;
            }

            if (value is double totalMilliseconds)
            {
                var timeSpan = TimeSpan.FromMilliseconds(totalMilliseconds);

                return string.Format("{0:D2}:{1:D2}", (int)timeSpan.TotalMinutes, timeSpan.Seconds) +
                    (needViewMilliseconds ? string.Format(".{0}", timeSpan.Milliseconds / 100) : "");
            }

            return DependencyProperty.UnsetValue;
        }
    }
}