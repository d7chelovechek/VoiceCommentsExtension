using System;
using System.Globalization;

namespace VoiceCommentsExtension.Converters
{
    public class BarBytesToBarOpacity : BaseMultiValueConverter
    {
        public override object Convert(
            object[] values,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (values.Length is 2 &&
                values[0] is long barCurrentPosition &&
                values[1] is long audioCurrentPosition) 
            { 
                return audioCurrentPosition >= barCurrentPosition ? 1d : 0.75d;
            }

            return 0.75d;
        }
    }
}