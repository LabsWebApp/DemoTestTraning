using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DemoTest.WpfView.Helpers.Converters;

public class BoolToForegroundColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var bValue = false; 
        if (value is bool b) bValue = b;
        else if (value is bool?) bValue = (bool)(bool?)value;
        return bValue ? Brushes.Green : Brushes.Red;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}