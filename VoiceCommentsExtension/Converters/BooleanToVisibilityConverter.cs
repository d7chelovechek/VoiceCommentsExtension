using System;
using System.Globalization;
using System.Windows;

namespace VoiceCommentsExtension.Converters
{
    public class BooleanToVisibilityConverter : BaseValueConverter
    {
        public override object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return 
                value is bool boolValue &&
                    parameter is string stringParameter &&
                    bool.TryParse(stringParameter, out bool valueToEqual) &&
                    boolValue.Equals(valueToEqual) ?
                Visibility.Visible : Visibility.Collapsed;
        }
    }
}