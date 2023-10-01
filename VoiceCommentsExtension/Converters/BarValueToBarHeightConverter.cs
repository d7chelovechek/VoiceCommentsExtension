using System;
using System.Globalization;
using System.Windows;

namespace VoiceCommentsExtension.Converters
{
    public class BarValueToBarHeightConverter : BaseMultiValueConverter
    {
        public override object Convert(
            object[] values, 
            Type targetType, 
            object parameter, 
            CultureInfo culture)
        {
            if (values.Length is 2 && 
                values[0] is double actualHeight && 
                values[1] is double barValue)
            {
                return barValue * actualHeight;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}