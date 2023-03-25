using System;
using System.Globalization;
using System.Windows.Data;

namespace DemoTest.WpfView.Helpers.Converters;

public class BoolToYesNoSymbolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture) =>
        value as bool? ?? false ? "" : "";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}