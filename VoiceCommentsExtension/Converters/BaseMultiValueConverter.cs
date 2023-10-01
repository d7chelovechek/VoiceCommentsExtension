using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace VoiceCommentsExtension.Converters
{
    public class BaseMultiValueConverter : IMultiValueConverter
    {
        public virtual object Convert(
            object[] values, 
            Type targetType, 
            object parameter, 
            CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        public virtual object[] ConvertBack(
            object value, 
            Type[] targetTypes, 
            object parameter, 
            CultureInfo culture)
        {
            return targetTypes.Select(t => DependencyProperty.UnsetValue).ToArray();
        }
    }
}