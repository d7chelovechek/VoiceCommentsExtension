using System;
using System.Globalization;
using System.Windows;

namespace VoiceCommentsExtension.Converters
{
    public class ChartWidthToBarWidthConverter : BaseMultiValueConverter
    {
        public override object Convert(
            object[] values, 
            Type targetType, 
            object parameter, 
            CultureInfo culture)
        {
            if (values.Length is 2 &&
                values[0] is double actualWidth &&
                values[1] is int barsCount)
            {
                return actualWidth / barsCount * 0.9;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}