using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DemoTest.WpfView.Helpers.Converters;

public class StringToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        var sValue = value switch
        {
            string => value,
            _ => string.Empty
        };
        return string.IsNullOrWhiteSpace((string)sValue) ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}