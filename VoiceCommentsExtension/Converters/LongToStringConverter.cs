using System;
using System.Globalization;
using System.Windows;

namespace VoiceCommentsExtension.Converters
{
    public class LongToStringConverter : BaseValueConverter
    {
        public override object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value is long longValue)
            {
                var timeSpan = TimeSpan.FromMilliseconds(longValue);

                return string.Format(
                    "{0}:{1}:{2}.{3}",
                    timeSpan.Hours, 
                    timeSpan.Minutes, 
                    timeSpan.Seconds, 
                    timeSpan.Milliseconds);
            }

            return DependencyProperty.UnsetValue;
        }
    }
}