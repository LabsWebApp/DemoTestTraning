using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using DemoTest.WpfView.Helpers.Converters;

namespace DemoTest.WpfView.Controls;

public class ShowErrorMessageLabel : Label
{
    public ShowErrorMessageLabel()
    {
        Foreground = Brushes.Red;
        var binding = new Binding
        {
            RelativeSource = RelativeSource.Self,
            Path = new PropertyPath(ContentProperty),
            Mode = BindingMode.OneWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            Converter = new StringToVisibilityConverter()
        };
        SetBinding(VisibilityProperty, binding);
    }
}