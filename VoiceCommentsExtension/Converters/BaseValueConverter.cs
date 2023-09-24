using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VoiceCommentsExtension.Converters
{
    public class BaseValueConverter : IValueConverter
    {
        public virtual object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        public virtual object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}