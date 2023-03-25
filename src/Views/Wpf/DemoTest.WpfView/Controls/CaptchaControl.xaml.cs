using DemoTest.WpfView.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DemoTest.WpfView.Controls
{
    /// <summary>
    /// Interaction logic for CaptchaControl.xaml
    /// </summary>
    public partial class CaptchaControl 
    {
        public CaptchaControl()
        {
            InitializeComponent();

            RegToggle toggle = new(
                @"Software\Microsoft\Input\Settings",
                "EnableHwkbTextPrediction", "MultilingualEnabled");

            TxtBox.GotFocus += (_, _) => toggle.Switch(RegToggle.Toggle.Off);
            TxtBox.LostFocus += (_, _) => toggle.Switch(RegToggle.Toggle.On);
        }

        private void Button_Click(object sender, RoutedEventArgs e) => TxtBox.Focus();

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e) => TxtBox.Focus();
    }
}
