using System;
using System.Globalization;
using System.Windows;

namespace VoiceCommentsExtension.Converters
{
    public class BarWidthToMarginConverter : BaseValueConverter
    {
        public override object Convert(
            object value, 
            Type targetType, 
            object parameter, 
            CultureInfo culture)
        {
            if (value is double width)
            {
                double margin = width * 0.05 / 0.9;

                return new Thickness(margin, 0, margin, 0);
            }

            return DependencyProperty.UnsetValue;
        }
    }
}